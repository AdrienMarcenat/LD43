using System.Collections.Generic;
using UnityEngine.Assertions;

using StateID = System.Type;


public class HSM
{
    private Stack<HSMState> m_StateStack;
    private Queue<HSMTransition> m_TransitionQueue;
    private Dictionary<StateID, HSMState> m_StateFactory;
    private bool m_IsStackChanged;

    public HSM (params HSMState[] states)
    {
        m_StateStack = new Stack<HSMState> ();
        m_TransitionQueue = new Queue<HSMTransition> ();
        m_StateFactory = new Dictionary<StateID, HSMState> ();
        m_IsStackChanged = false;

        foreach (HSMState state in states)
        {
            m_StateFactory.Add (state.GetType (), state);
        }
    }

    public void Start (StateID firstStateID)
    {
        PushState (firstStateID);
        UpdaterProxy.Get ().Register (this, EUpdatePass.Last);
        DumpStack ();
    }

    public void Stop ()
    {
        DumpStack ();
        UpdaterProxy.Get ().Unregister (this, EUpdatePass.Last);
        ClearStack ();
        m_TransitionQueue.Clear ();
    }

    public void UpdateLast ()
    {
        m_IsStackChanged = false;
        foreach (HSMState state in m_StateStack)
        {
            m_TransitionQueue.Enqueue (state.EvalTransition ());
        }
        ApplyHSMTransitions ();
        if (m_IsStackChanged)
        {
            DumpStack ();
        }
    }

    public bool IsEmpty ()
    {
        return m_StateStack.Count == 0;
    }

    private HSMState FindState (StateID stateID)
    {
        HSMState state = null;
        if (!m_StateFactory.TryGetValue (stateID, out state))
        {
            Assert.IsTrue (false, "cannot find state with ID " + stateID);
        }
        return state;
    }

    private void ApplyHSMTransitions ()
    {
        while (m_TransitionQueue.Count != 0)
        {
            HSMTransition transition = m_TransitionQueue.Dequeue ();
            switch (transition.m_Type)
            {
                case HSMTransition.EType.Clear:
                    // Don't apply other transition as we completly change the stack
                    m_TransitionQueue.Clear ();
                    ClearStack ();
                    PushState (transition.m_DestinationID);
                    break;
                case HSMTransition.EType.Siblings:
                    ReplaceState (transition.m_SourceID, transition.m_DestinationID);
                    break;
                case HSMTransition.EType.Child:
                    HSMState peekState = m_StateStack.Peek ();
                    if (peekState.GetID () == transition.m_SourceID)
                    {
                        PushState (transition.m_DestinationID);
                    }
                    break;
                case HSMTransition.EType.Exit:
                    ReplaceState (transition.m_SourceID, null);
                    break;
                case HSMTransition.EType.Top:
                    PushState (transition.m_DestinationID);
                    break;
                case HSMTransition.EType.None:
                    break;
                default:
                    Assert.IsTrue (false, "Invalid transition type");
                    break;
            }
        }
    }

    private void PushState (StateID stateID)
    {
        if (stateID != null)
        {
            HSMState newState = FindState (stateID);
            Assert.IsTrue (newState != null, "Cannot find state with ID " + stateID);
            newState.OnEnter ();
            m_StateStack.Push (newState);
            m_IsStackChanged = true;
        }
    }

    private void ReplaceState (StateID oldStateID, StateID newStateiD)
    {
        List<HSMState> topStates = new List<HSMState> ();
        for (int i = 0; i < m_StateStack.Count; i++)
        {
            // Save all the states above
            if (m_StateStack.Peek ().GetID () != oldStateID)
            {
                topStates.Add (m_StateStack.Pop ());
            }
            else
            {
                m_StateStack.Pop ().OnExit ();
                m_IsStackChanged = true;
                break;
            }
        }
        PushState (newStateiD);
        // Re push all the states above
        for (int i = topStates.Count - 1; i >= 0; i--)
        {
            m_StateStack.Push (topStates[i]);
        }
    }

    private void ClearStack ()
    {
        while (m_StateStack.Count != 0)
        {
            m_StateStack.Pop ().OnExit ();
        }
        m_IsStackChanged = true;
    }

    private void DumpStack ()
    {
        this.DebugLog ("=======================> Dump state stack begin");
        foreach (HSMState state in m_StateStack)
        {
            this.DebugLog (state.GetType ());
        }
        this.DebugLog ("<======================= Dump state stack end");
    }

    public HSMState[] GetStack()
    {
        return m_StateStack.ToArray ();
    }

    public Dictionary<StateID, HSMState> GetStates ()
    {
        return m_StateFactory;
    }
}


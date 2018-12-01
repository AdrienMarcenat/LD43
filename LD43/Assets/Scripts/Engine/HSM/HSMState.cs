using UnityEngine;
using System.Collections;

using StateID = System.Type;

public struct HSMTransition
{
    public enum EType
    {
        Child,
        Siblings,
        Exit,
        Clear,
        None
    };

    public EType m_Type;
    public StateID m_SourceID;
    public StateID m_DestinationID;

    public HSMTransition (EType type = EType.None, StateID sourceID = null, StateID destinationID = null)
    {
        m_Type = type;
        m_SourceID = sourceID;
        m_DestinationID = destinationID;
    }
}

public abstract class HSMState
{
    private HSMTransition m_NextTransition;
    private StateID m_ID;

    public HSMState()
    {
        m_ID = this.GetType ();
    }

    public StateID GetID ()
    {
        return m_ID;
    }

    public virtual void OnEnter () { }
    public virtual bool OnUpdate () { return false; }
    public virtual void OnExit () { }

    public HSMTransition EvalTransition ()
    {
        HSMTransition res = m_NextTransition;
        m_NextTransition = new HSMTransition ();
        return res;
    }

    protected void ChangeNextTransition (HSMTransition.EType type, StateID destinationID = null)
    {
        m_NextTransition = new HSMTransition (type, m_ID, destinationID);
    }
}


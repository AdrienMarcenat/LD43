using System.Collections.Generic;
using UnityEngine.Assertions;

public class GameEventManager
{
    private Dictionary<string, ListenerNotifier> m_Notifiers;
    private Queue<GameEvent> m_GameEventQueue;
    private bool m_DispatchGuard = false;

    public GameEventManager ()
    {
        m_Notifiers = new Dictionary<string, ListenerNotifier> ();
        m_GameEventQueue = new Queue<GameEvent> ();
        UpdaterProxy.Get ().Register (this, EUpdatePass.First);
    }

    public void Register (System.Object objectToNotify, string tag, params System.Type[] GameEventTypes)
    {
        Assert.IsFalse (m_DispatchGuard, "Cannot register a listener while dispatching !");
        ListenerNotifier notifier = null;

        if (!m_Notifiers.TryGetValue (tag, out notifier))
        {
            notifier = new ListenerNotifier (tag);
            m_Notifiers.Add (tag, notifier);
        }

        Assert.IsTrue (notifier != null);
        notifier.AddListener (objectToNotify, tag, GameEventTypes);
    }

    public void Unregister (System.Object objectToNotify, string tag)
    {
        Assert.IsFalse (m_DispatchGuard, "Cannot unregister a listener while dispatching !");
        ListenerNotifier notifier = null;

        if (!m_Notifiers.TryGetValue (tag, out notifier))
        {
            Assert.IsTrue (false, "Trying to unregister to GameEvents from " + tag + " but no notifier was found.");
        }

        notifier.RemoveListener (objectToNotify, tag);
    }

    public void PushGameEvent (GameEvent e, GameEvent.EProtocol protocol)
    {
        //Assert.IsFalse (m_DispatchGuard, "Cannot push GameEvent while dispatching !");
        switch (protocol)
        {
            case GameEvent.EProtocol.Delayed:
                m_GameEventQueue.Enqueue (e);
                break;
            case GameEvent.EProtocol.Instant:
                Notify (e);
                break;
            case GameEvent.EProtocol.Discard:
                break;
            default:
                Assert.IsTrue (false, "Invalid GameEvent protocol");
                break;
        }
    }

    public void UpdateFirst ()
    {
        m_DispatchGuard = true;
        List<GameEvent> eventToDispatch = new List<GameEvent> ();
        while (m_GameEventQueue.Count != 0)
        {
            eventToDispatch.Add (m_GameEventQueue.Dequeue ());
        }
        foreach (GameEvent gameEvent in eventToDispatch)
        {
            Notify (gameEvent);
        }
        eventToDispatch.Clear ();
        m_DispatchGuard = false;
    }

    private void Notify (GameEvent e)
    {
        string tag = e.GetTag ();
        ListenerNotifier notifier = null;

        if (m_Notifiers.TryGetValue (tag, out notifier))
        {
            notifier.Notify (e);
        }
    }
}

public class GameEventManagerProxy : UniqueProxy<GameEventManager>
{ }
using System.Collections.Generic;
using UnityEngine.Assertions;

public class ListenerNotifier
{
    private string m_Tag;
    private List<Listener> m_Listeners;

    public ListenerNotifier (string tag)
    {
        m_Tag = tag;
        m_Listeners = new List<Listener> ();
    }

    public void Notify (GameEvent e)
    {
        Assert.IsTrue (e.GetTag () == m_Tag, "GameEvent has tag " + e.GetTag () + " but notifier has tag " + m_Tag);
        foreach (Listener listener in m_Listeners)
        {
            if (listener.IsGameEventHandled (e))
            {
                ReflectionHelper.CallMethod ("OnGameEvent", listener.GetObjectToNotify(), e);
            }
        }
    }

    public void AddListener (System.Object objectToNotify, string tag, params System.Type[] GameEventTypes)
    {
        m_Listeners.Add (new Listener(objectToNotify, tag, GameEventTypes));
    }

    public void RemoveListener (System.Object objectToNotify, string tag)
    {
        int indexToRemove = m_Listeners.FindIndex (
            delegate(Listener listener)
            {
                return listener.GetTag () == tag && listener.GetObjectToNotify () == objectToNotify;
            }
        );
        if(indexToRemove < m_Listeners.Count && indexToRemove >= 0)
        {
            m_Listeners.RemoveAt (indexToRemove);
        }
    }
}
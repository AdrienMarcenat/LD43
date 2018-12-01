using UnityEngine;

public class Listener
{
    private System.Object m_ObjectToNotify;
    private string m_Tag;
    private System.Type[] m_GameEventTypes;

    public Listener (System.Object objectToNotify, string tag, params System.Type[] GameEventTypes)
    {
        m_ObjectToNotify = objectToNotify;
        m_Tag = tag;
        m_GameEventTypes = GameEventTypes;
    }

    public System.Object GetObjectToNotify ()
    {
        return m_ObjectToNotify;
    }

    public string GetTag ()
    {
        return m_Tag;
    }

    public bool IsGameEventHandled (GameEvent e)
    {
        System.Type GameEventType = e.GetType ();
        foreach (System.Type type in m_GameEventTypes)
        {
            if (type == GameEventType)
            {
                return true;
            }
        }
        return false;
    }
}
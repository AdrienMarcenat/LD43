public class GameEvent
{
    public enum EProtocol
    {
        Discard,
        Instant,
        Delayed,
    }

    private string m_Tag;
    private EProtocol m_Protocol;

    public GameEvent (string tag, EProtocol protocol = EProtocol.Delayed)
    {
        m_Tag = tag;
        m_Protocol = protocol;
    }

    public string GetTag ()
    {
        return m_Tag;
    }

    public void Push()
    {
        GameEventManagerProxy.Get ().PushGameEvent (this, m_Protocol);
    }
}
public interface LoggerInterface
{
    void Log (object message);
}

public class UnityLogger : LoggerInterface
{
    public void Log (object message)
    {
        UnityEngine.Debug.Log (message);
    }
}

public class NullLogger : LoggerInterface
{
    public void Log (object message)
    {
    }
}

public class LoggerProxy : UniqueProxy<LoggerInterface>
{ }
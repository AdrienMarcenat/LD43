using UnityEngine.Assertions;

public class UniqueProxy<T> where T : class
{
    public static void Open (T instance)
    {
        Assert.IsTrue (ms_Instance == null, "Proxy already set !");
        ms_Instance = instance;
    }

    public static void Close ()
    {
        Assert.IsTrue (ms_Instance != null, "Proxy wasn't set");
        ms_Instance = null;
    }

    public static T Get ()
    {
        Assert.IsTrue (ms_Instance != null, "Invalid proxy");
        return ms_Instance;
    }

    public static bool IsValid()
    {
        return ms_Instance != null;
    }

    private static T ms_Instance;
}
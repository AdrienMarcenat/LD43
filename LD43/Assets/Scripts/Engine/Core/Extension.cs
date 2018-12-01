
using System;
using UnityEngine;

public static class Extension
{
    public static void RegisterAsListener (this System.Object objectToNotify, string tag, params System.Type[] GameEventTypes)
    {
        GameEventManagerProxy.Get ().Register (objectToNotify, tag, GameEventTypes);
    }

    public static void UnregisterAsListener (this System.Object objectToNotify, string tag)
    {
        GameEventManagerProxy.Get ().Unregister (objectToNotify, tag);
    }

    public static void RegisterToUpdate (this System.Object objectToNotify, params EUpdatePass[] updatePassList)
    {
        UpdaterProxy.Get ().Register (objectToNotify, updatePassList);
    }

    public static void UnregisterToUpdate (this System.Object objectToNotify, params EUpdatePass[] updatePassList)
    {
        UpdaterProxy.Get ().Unregister (objectToNotify, updatePassList);
    }

    public static void SetX (this Vector3 v, float newX)
    {
        v.Set (newX, v.y, v.z);
    }

    public static void SetY (this Vector3 v, float newY)
    {
        v.Set (v.x, newY, v.z);
    }

    public static void SetZ (this Vector3 v, float newZ)
    {
        v.Set (v.x, v.y, newZ);
    }

    public static void DebugLog (this System.Object caller, System.Object message)
    {
        LoggerProxy.Get ().Log (message);
    }

    public static T[] SubArray<T> (this T[] data, int index, int length = -1)
    {
        if (length == -1)
        {
            length = data.Length - index;
        }
        T[] result = new T[length];
        Array.Copy (data, index, result, 0, length);
        return result;
    }
}
using System.Collections.Generic;
using UnityEngine.Assertions;

using AnyObject = System.Object;

public enum EUpdatePass
{
    First,
    BeforeAI,
    AI,
    AfterAI,
    Last,
    // Do not move count !!!
    Count
}

public class Updater
{
    List<AnyObject>[] m_ObjectListPerPass;
    private bool m_UpdateGuard = false;
    private int m_PauseLock = 0;

    public Updater ()
    {
        m_ObjectListPerPass = new List<AnyObject>[(int)EUpdatePass.Count];
        for (int i = 0; i < (int)EUpdatePass.Count; i++)
        {
            m_ObjectListPerPass[i] = new List<AnyObject> ();
        }
    }

    public void Update ()
    {
        /*if (m_IsPaused)
        {
            return;
        }*/

        m_UpdateGuard = true;
        for (int i = 0; i < (int)EUpdatePass.Count; i++)
        {
            EUpdatePass pass = (EUpdatePass)i;
            foreach (AnyObject objectToUpdate in m_ObjectListPerPass[(int)pass])
            {
                ReflectionHelper.CallMethod ("Update" + pass.ToString (), objectToUpdate);
            }
        }
        m_UpdateGuard = false;
    }

    public void Register (AnyObject objectToUpdate, params EUpdatePass[] updatePassList)
    {
        Assert.IsFalse (m_UpdateGuard, "Cannot register a listener while updating !");
        foreach (EUpdatePass pass in updatePassList)
        {
            Assert.IsTrue (pass != EUpdatePass.Count, "Invalid Update Pass : " + pass.ToString ());
            if (!m_ObjectListPerPass[(int)pass].Contains (objectToUpdate))
            {
                m_ObjectListPerPass[(int)pass].Add (objectToUpdate);
            }
        }
    }

    public void Unregister (AnyObject objectToUpdate, params EUpdatePass[] updatePassList)
    {
        Assert.IsFalse (m_UpdateGuard, "Cannot unregister a listener while updating !");
        foreach (EUpdatePass pass in updatePassList)
        {
            Assert.IsTrue (pass != EUpdatePass.Count, "Invalid Update Pass : " + pass.ToString ());
            m_ObjectListPerPass[(int)pass].Remove (objectToUpdate);
        }
    }

    public void SetPause (bool pause)
    {
        m_PauseLock += pause ? 1 : -1;
        m_IsPaused = m_PauseLock > 0;
    }

    public bool IsPaused ()
    {
        return m_IsPaused;
    }

    private bool m_IsPaused = false;
}

public class UpdaterProxy : UniqueProxy<Updater>
{}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEvent : GameEvent
{
    public LevelEvent (int levelIndex, bool enter) : base ("Game", EProtocol.Instant)
    {
        m_LevelIndex = levelIndex;
        m_Enter = enter;
    }

    public int GetLevelIndex ()
    {
        return m_LevelIndex;
    }

    public bool IsEntered ()
    {
        return m_Enter;
    }

    private int m_LevelIndex;
    private bool m_Enter;
}

public class LevelManager
{
    private int m_CurrentLevel = 0;
    private static int ms_MaxLevel = 3;

    public void LoadScene (int sceneIndex)
    {
        new LevelEvent (m_CurrentLevel, false).Push ();
        SceneManager.LoadScene (sceneIndex);
    }

    public void LoadScene (string sceneName)
    {
        new LevelEvent (m_CurrentLevel, false).Push ();
        SceneManager.LoadScene (sceneName);
    }

    public void LoadLevel ()
    {
        LoadScene ("Scenes/Levels/Level" + m_CurrentLevel);
    }

    public void SetLevelIndex (int levelIndex)
    {
        m_CurrentLevel = levelIndex;
    }

    public bool IsLastLevel ()
    {
        return m_CurrentLevel == ms_MaxLevel;
    }

    public void NextLevel ()
    {
        if (!IsLastLevel ())
        {
            m_CurrentLevel++;
        }
    }

    public string GetActiveSceneName ()
    {
        return SceneManager.GetActiveScene ().name;
    }

    public int GetCurrentLevelID ()
    {
        return m_CurrentLevel;
    }
}

public class LevelManagerProxy : UniqueProxy<LevelManager>
{ }

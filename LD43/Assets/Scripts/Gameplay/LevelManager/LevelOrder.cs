using System.Collections.Generic;
using UnityEngine;

public struct LevelSequence
{
    public enum ELevelSequenceStart
    {
        NOW,
        WHEN_DONE,
        ERROR
    }

    public ELevelSequenceStart m_Start;
    public List<LevelOrder> m_Orders;

    public LevelSequence (ELevelSequenceStart start = ELevelSequenceStart.ERROR)
    {
        m_Start = start;
        m_Orders = new List<LevelOrder> ();
    }

    static public ELevelSequenceStart Parse (string type)
    {
        switch (type.ToLower ())
        {
            case ("[now]"):
                return ELevelSequenceStart.NOW;
            case ("[done]"):
                return ELevelSequenceStart.WHEN_DONE;
        }
        return ELevelSequenceStart.ERROR;
    }
}

public abstract class LevelOrder
{
    private float m_Time;

    public LevelOrder (float time = 0)
    {
        m_Time = time;
    }

    public float GetTime ()
    {
        return m_Time;
    }

    public abstract void ExecuteOrder ();
}

public class SpawnLevelOrder : LevelOrder
{
    private List<GameObject> m_Entities = new List<GameObject> ();
    private List<int> m_EntitiesQuantity = new List<int> ();

    public SpawnLevelOrder (string[] args, float time) : base (time)
    {
        foreach (string order in args)
        {
            string[] things = order.Split ('*');
            string prefab;
            int num = 1;
            if (things.Length > 1)
            {
                num = int.Parse (things[0]);
                prefab = things[1];
            }
            else
            {
                prefab = things[0];
            }
            m_Entities.Add (RessourceManager.LoadPrefab (prefab));
            m_EntitiesQuantity.Add (num);
        }
    }

    public override void ExecuteOrder ()
    {
        for (int i = 0; i < m_Entities.Count; i++)
        {
            for (int k = 0; k < m_EntitiesQuantity[i]; k++)
            {
                GameObject.Instantiate (m_Entities[i]);
            }
        }
    }
}

public class MusicLevelOrder : LevelOrder
{
    private string m_MusicName;

    public MusicLevelOrder (string music, float time) : base (time)
    {
        m_MusicName = music;
    }

    public override void ExecuteOrder ()
    {
        this.DebugLog ("MUSIC!!!");
    }
}

public class TalkLevelOrder : LevelOrder
{
    private string m_Tag;

    public TalkLevelOrder (string tag, float time) : base (time)
    {
        m_Tag = tag;
    }

    public override void ExecuteOrder ()
    {
        DialogueManagerProxy.Get ().TriggerDialogue (m_Tag);
    }
}

public class EndLevelOrder : LevelOrder
{
    public EndLevelOrder (float time) : base (time)
    {
    }

    public override void ExecuteOrder ()
    {
        this.DebugLog ("END LEVEL!!!");
    }
}
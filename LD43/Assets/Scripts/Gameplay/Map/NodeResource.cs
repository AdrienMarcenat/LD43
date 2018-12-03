using UnityEngine;
using System.Collections.Generic;

public enum ENodeType
{
    Normal,
    Recruitment,
    Key,
    Battle,
    End
}

public enum ENodeReward
{
    None,
    Heal
}

[System.Serializable]
public class NodeResource
{
    [SerializeField] private ENodeType m_NodeType = ENodeType.Normal;
    [SerializeField] private int m_NodeId;
    [SerializeField] private string m_DialogueTag;
    [SerializeField] private ENodeReward m_NodeReward = ENodeReward.None;
    [SerializeField] private List<ECharacterClass> m_Enemies;
    [SerializeField] private bool m_NeedKey = false;

    public void SetNodeId (int newNodeId)
    {
        m_NodeId = newNodeId;
    }

    public ENodeType GetNodeType ()
    {
        return m_NodeType;
    }

    public int GetNodeId ()
    {
        return m_NodeId;
    }

    public string GetDialogueTag ()
    {
        return m_DialogueTag;
    }

    public ENodeReward GetNodeReward ()
    {
        return m_NodeReward;
    }

    public List<ECharacterClass> GetEnemies ()
    {
        return m_Enemies;
    }

    public bool NeedKey ()
    {
        return m_NeedKey;
    }
}

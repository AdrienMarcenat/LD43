using UnityEngine;

[System.Serializable]
public class NodeResource
{
    [SerializeField] private int m_NodeType;
    [SerializeField] private int m_NodeId;
    [SerializeField] private string m_DialogueTag;

    public void SetNodeId (int newNodeId)
    {
        m_NodeId = newNodeId;
    }

    public int GetNodeType ()
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
}

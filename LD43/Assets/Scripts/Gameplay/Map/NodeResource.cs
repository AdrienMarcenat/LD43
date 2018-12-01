
public class NodeResource
{
    private int m_NodeType;
    private int m_NodeId;

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
}

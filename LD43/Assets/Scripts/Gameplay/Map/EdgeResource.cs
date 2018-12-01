
public class EdgeResource
{
    private int m_EdgeType;
    private int m_EdgeId;
    
    public void SetEdgeId(int newEdgeId)
    {
        m_EdgeId = newEdgeId;
    }

    public int GetEdgeType ()
    {
        return m_EdgeType;
    }

    public int GetEdgeId ()
    {
        return m_EdgeId;
    }
}

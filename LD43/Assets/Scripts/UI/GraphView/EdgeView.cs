using UnityEngine;

public class EdgeView : MonoBehaviour
{
    private GameEdge m_Edge;

    [SerializeField] private NodeView m_Start;
    [SerializeField] private NodeView m_End;
    [SerializeField] private bool m_IsOriented;
    [SerializeField] private Sprite m_Sprite;

    public void BuildEdge()
    {
        if(m_Edge != null)
        {
            m_Edge.Shutdown ();
        }
        if (IsValid ())
        {
            m_Edge = new GameEdge (null, m_Start.GetNode (), m_End.GetNode (), m_IsOriented);
        }
    }

    public GameEdge GetEdge()
    {
        return m_Edge;
    }

    public NodeView GetStart()
    {
        return m_Start;
    }

    public NodeView GetEnd ()
    {
        return m_End;
    }
    
    public void SetStart (NodeView start)
    {
        m_Start = start;
    }

    public void SetEnd (NodeView end)
    {
        m_End = end;
    }

    public bool IsOriented()
    {
        return m_IsOriented;
    }

    public void SetIsOriented (bool isOriented)
    {
        m_IsOriented = isOriented;
    }
    
    public bool IsValid()
    {
        return m_Start != null && m_End != null;
    }
}

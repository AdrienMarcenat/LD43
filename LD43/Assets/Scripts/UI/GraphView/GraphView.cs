using UnityEngine;
using System.Collections.Generic;

public class GraphView : MonoBehaviour
{
    private Graph m_Graph;
    [SerializeField] private List<NodeView> m_Nodes;
    [SerializeField] private List<EdgeView> m_Edges;

    void Awake ()
    {
        m_Graph = new Graph ();
        m_Nodes = new List<NodeView> ();
        m_Edges = new List<EdgeView> ();
    }

    public Graph GetGraph ()
    {
        return m_Graph;
    }

    public List<NodeView> GetNodes()
    {
        return m_Nodes;
    }

    public List<EdgeView> GetEdges ()
    {
        return m_Edges;
    }
}

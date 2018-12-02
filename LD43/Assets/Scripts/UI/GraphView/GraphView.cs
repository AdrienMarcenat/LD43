using UnityEngine;
using System.Collections.Generic;

public class GraphView : MonoBehaviour
{
    private Graph m_Graph;

    [SerializeField] private List<NodeView> m_Nodes;
    [SerializeField] private List<EdgeView> m_Edges;

    void Awake ()
    {
        BuildGraph ();
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

    private void BuildGraph ()
    {
        m_Graph = new Graph ();
        foreach (NodeView node in m_Nodes)
        {
            node.BuildNode ();
            m_Graph.AddNode (node.GetNode ());
        }
        foreach (EdgeView edge in m_Edges)
        {
            edge.ResizeCollider ();
            edge.BuildEdge ();
            m_Graph.AddEdge (edge.GetEdge ());
        }
    }
}

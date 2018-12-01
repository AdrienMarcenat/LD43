using UnityEngine;
using System.Collections.Generic;

public class GraphView : MonoBehaviour
{
    private Graph m_Graph;
    [SerializeField] private GameObject m_NodePrefab;
    [SerializeField] private GameObject m_EdgePrefab;
    private List<NodeView> m_Nodes;
    private List<EdgeView> m_Edges;

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

    public void AddNodes ()
    {
        m_Nodes.Add (Instantiate (m_NodePrefab).GetComponent<NodeView> ());
    }

    public void RemoveNode (int index)
    {
        GameObject node = m_Nodes[index].gameObject;
        m_Nodes.RemoveAt (index);
        DestroyImmediate (node);
    }

    public void AddEdge ()
    {
        m_Edges.Add (Instantiate (m_EdgePrefab).GetComponent<EdgeView> ());
    }

    public void RemoveEdge (int index)
    {
        GameObject edge = m_Edges[index].gameObject;
        m_Edges.RemoveAt (index);
        DestroyImmediate (edge);
    }
}

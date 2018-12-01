using System.Collections.Generic;
using UnityEngine.Assertions;

public class Node<T>
{
    private T m_Data;
    private List<Node<T>> m_Neighbors;

    public Node ()
    {}
    public Node (T data)
    {
        m_Neighbors = new List<Node<T>> ();
        m_Data = data;
    }
    public Node (T data, List<Node<T>> neighbors)
        : this(data)
    {
        m_Neighbors = neighbors;
    }

    public T GetData()
    {
        return m_Data;
    }

    public List<Node<T>> GetNeigbors ()
    {
        return m_Neighbors;
    }

    public void AddNeigbor(Node<T> node)
    {
        m_Neighbors.Add (node);
    }

    public bool RemoveNeigboor(Node<T> node)
    {
        return m_Neighbors.Remove (node);
    }

    public bool IsNeighbor(Node<T> node)
    {
        return m_Neighbors.Contains (node);
    }
}

public class Edge<NodeData, EdgeData>
{
    private EdgeData m_Data;
    private Node<NodeData> m_Start;
    private Node<NodeData> m_End;
    private bool m_IsOriented;

    public Edge ()
    {}
    public Edge (EdgeData data, Node<NodeData> start, Node<NodeData> end, bool isOriented = false)
    {
        m_Data = data;
        m_Start = start;
        m_End = end;
        m_IsOriented = isOriented;

        Assert.IsFalse (m_Start.IsNeighbor(end) || m_End.IsNeighbor(start));
        m_Start.AddNeigbor (m_End);
        if(!m_IsOriented)
        {
            m_End.AddNeigbor (m_Start);
        }
    }

    public void Shutdown()
    {
        m_Start.RemoveNeigboor (m_End);
        if (!m_IsOriented)
        {
            m_End.RemoveNeigboor (m_Start);
        }
    }

    public EdgeData GetData ()
    {
        return m_Data;
    }

    public bool UseNode(Node<NodeData> node)
    {
        return m_Start == node || m_End == node;
    }

    public bool IsOriented ()
    {
        return m_IsOriented;
    }
    
    public Node<NodeData> GetStart()
    {
        return m_Start;
    }

    public Node<NodeData> GetEnd ()
    {
        return m_End;
    }
}

public class Graph<NodeData, EdgeData>
{
    private List<Node<NodeData>> m_Nodes;
    private List<Edge<NodeData, EdgeData>> m_Edges;

    public Graph ()
    {
        m_Nodes = new List<Node<NodeData>>();
        m_Edges = new List<Edge<NodeData, EdgeData>> ();
    }

    public void AddNode (Node<NodeData> node)
    {
        m_Nodes.Add (node);
    }
    
    public void AddEdge (Edge<NodeData, EdgeData> edge)
    {
        Assert.IsTrue (Contains (edge.GetStart ()) && Contains (edge.GetEnd ()));
        m_Edges.Add (edge);
    }

    public bool RemoveNode (Node<NodeData> node)
    {
        if(m_Nodes.Remove (node))
        {
            // Remove all edges using this node
            for (int i = m_Edges.Count - 1; i >= 0; ++i)
            {
                Edge<NodeData, EdgeData> edge = m_Edges[i];
                if (edge.UseNode(node))
                {
                    edge.Shutdown ();
                    m_Edges.RemoveAt (i);
                }
            }
            return true;
        }

        return false;
    }

    public bool RemoveEdge (Edge<NodeData, EdgeData> edge)
    {
        edge.Shutdown ();
        return m_Edges.Remove (edge);
    }

    public bool Contains(Node<NodeData> node)
    {
        return m_Nodes.Contains (node);
    }

    public bool Contains (Edge<NodeData, EdgeData> edge)
    {
        return m_Edges.Contains (edge);
    }

    public List<Node<NodeData>> GetNodes ()
    {
        return m_Nodes;
    }

    public List<Edge<NodeData, EdgeData>> GetEdges ()
    {
        return m_Edges;
    }
}

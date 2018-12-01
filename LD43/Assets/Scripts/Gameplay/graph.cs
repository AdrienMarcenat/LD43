using System.Collections.Generic;

public class Node<T>
{
    private T m_Data;
    private List<Node<T>> m_Neighbors = null;

    public Node ()
    {}
    public Node (T data) 
        : this (data, null)
    {}
    public Node (T data, List<Node<T>> neighbors)
    {
        this.m_Data = data;
        this.m_Neighbors = neighbors;
    }
}

public class Edge<NodeData, EdgeData>
{
    private EdgeData m_Data;
    private Node<NodeData> m_Start;
    private Node<NodeData> m_End;
    private bool m_IsOriendted;
    
    public Edge (EdgeData data, Node<NodeData> start, Node<NodeData> end, bool isOriented = false)
    {
        this.m_Data = data;
        this.m_Start = start;
        this.m_End = end;
        this.m_IsOriendted = isOriented;
    }

    public bool UseNode(Node<NodeData> node)
    {
        return m_Start == node || m_End == node;
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
                    m_Edges.RemoveAt (i);
                }
            }
        }

        return true;
    }
}

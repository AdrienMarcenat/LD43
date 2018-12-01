using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor (typeof (GraphView))]
public class GraphInspector : Editor
{
    private GraphView m_Graph;
    private Transform m_HandleTransform;
    private Quaternion m_HandleRotation;

    private void OnSceneGUI ()
    {
        m_Graph = target as GraphView;
        m_HandleTransform = m_Graph.transform;
        m_HandleRotation = Tools.pivotRotation == PivotRotation.Local ? m_HandleTransform.rotation : Quaternion.identity;

        foreach (NodeView node in m_Graph.GetNodes ())
        {
            if(node != null)
                ShowNode (node);
        }
        List<EdgeView> edges = m_Graph.GetEdges ();
        for (int i = 0; i < edges.Count; i++)
        {
            EdgeView edge = edges[i];
            if (edge != null && edge.IsValid ())
            {
                if(ShowEdge (edge))
                {
                    m_Graph.RemoveEdge (i);
                }
            }
        }
    }

    private void ShowNode (NodeView node)
    {
        Vector2 position = m_HandleTransform.TransformPoint (node.transform.position);
        Quaternion rotation = node.transform.rotation;
        EditorGUI.BeginChangeCheck ();
        position = Handles.DoPositionHandle (position, rotation);
        if (EditorGUI.EndChangeCheck ())
        {
            Undo.RecordObject (m_Graph, "Move Point");
            EditorUtility.SetDirty (m_Graph);
            node.transform.position = m_HandleTransform.InverseTransformPoint (position);
        }
    }

    private bool ShowEdge (EdgeView edge)
    {
        Handles.color = edge.IsOriented () ? Color.red : Color.green;
        Vector2 start = edge.GetStart ().transform.position;
        Vector2 end = edge.GetEnd ().transform.position;
        if (edge.IsOriented ())
        {
            Handles.DrawDottedLine (start, end, 5);
        }
        else
        {
            Handles.DrawLine (start, end);
        }
        return Handles.Button ((start + end) / 2, Quaternion.identity, 0.5f, 1, Handles.RectangleHandleCap);
    }

    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();

        m_Graph = target as GraphView;

        EditorGUILayout.BeginVertical (EditorStyles.helpBox);
        GUILayout.Label ("List of Nodes", EditorStyles.boldLabel);

        List<NodeView> nodes = m_Graph.GetNodes ();
        List<int> nodesToDestroy = new List<int>();
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] != null)
            {
                EditorGUILayout.BeginHorizontal ();
                if (GUILayout.Button ("Remove Node"))
                {
                    nodesToDestroy.Add (i);
                }
                GUILayout.Space (15);
                nodes[i].SetTitle (EditorGUILayout.TextField ("Name", nodes[i].GetTitle ()));
                EditorGUILayout.EndHorizontal ();
            }
        }
        if (GUILayout.Button ("Add Node"))
        {
            m_Graph.AddNodes ();
        }
        EditorGUILayout.EndVertical ();
        foreach (int i in nodesToDestroy)
        {
            m_Graph.RemoveNode (i);
        }

        EditorGUILayout.BeginVertical (EditorStyles.helpBox);
        GUILayout.Label ("List of Edges", EditorStyles.boldLabel);

        List<EdgeView> edges = m_Graph.GetEdges ();
        List<int> edgesToDestroy = new List<int> ();
        for (int i = 0; i < edges.Count; i++) 
        {
            if (edges[i] != null)
            {
                EditorGUILayout.BeginHorizontal ();

                EditorGUILayout.BeginVertical ();
                edges[i].SetStart ((NodeView)EditorGUILayout.ObjectField ("Start", edges[i].GetStart (), typeof (NodeView), true));
                edges[i].SetEnd ((NodeView)EditorGUILayout.ObjectField ("End", edges[i].GetEnd (), typeof (NodeView), true));
                edges[i].SetIsOriented (EditorGUILayout.Toggle ("IsOriented", edges[i].IsOriented ()));
                if (GUILayout.Button ("Remove Edge"))
                {
                    edgesToDestroy.Add (i);
                }
                EditorGUILayout.EndVertical ();
                
                EditorGUILayout.EndHorizontal ();

                EditorGUILayout.Separator ();
            }
        }
        if (GUILayout.Button ("Add Edge"))
        {
            m_Graph.AddEdge ();
        }
        EditorGUILayout.EndVertical ();
        foreach (int i in edgesToDestroy)
        {
            m_Graph.RemoveEdge (i);
        }
        SceneView.RepaintAll ();
    }
}
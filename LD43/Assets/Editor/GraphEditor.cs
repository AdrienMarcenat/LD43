using UnityEditor;
using UnityEngine;

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

        foreach (NodeView node in m_Graph.GetNodes())
        {
            ShowNode (node);
        }
        foreach (EdgeView edge in m_Graph.GetEdges ())
        {
            ShowEdge (edge);
        }
        //Handles.color = Color.gray;
        //Handles.DrawLine (p0, p1);
        //Handles.DrawLine (p2, p3);

        //ShowDirections ();
        //Handles.DrawBezier (p0, p3, p1, p2, Color.white, null, 2f);
    }

    private void ShowNode (NodeView node)
    {
        Vector2 posititon = m_HandleTransform.TransformPoint (node.transform.position);
        Quaternion rotation = node.transform.rotation;
        EditorGUI.BeginChangeCheck ();
        posititon = Handles.DoPositionHandle (posititon, rotation);
        if (EditorGUI.EndChangeCheck ())
        {
            Undo.RecordObject (m_Graph, "Move Point");
            EditorUtility.SetDirty (m_Graph);
            node.transform.position = m_HandleTransform.InverseTransformPoint (posititon);
        }
    }

    private void ShowEdge (EdgeView edge)
    {
        Handles.color = edge.IsOriented () ? Color.red : Color.green;
        Vector2 start = edge.GetStart().transform.position;
        Vector2 end = edge.GetEnd ().transform.position;
        if (edge.IsOriented ())
        {
            Handles.DrawDottedLine (start, end, 5);
            DrawTriangle(end);
        }
        else
        {
            Handles.DrawLine (start, end);
        }
    }

    private void DrawTriangle (Vector2 point)
    {
        float x = point.x;
        float y = point.y;
        Vector3[] points = { new Vector3(x,y,0), new Vector3 (x, y-1, 0), new Vector3 (x-1, y-1, 0), new Vector3 (x - 1, y, 0) };
        Handles.DrawAAConvexPolygon (points);
    }
}
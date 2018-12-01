//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor (typeof (List<NodeView>))]
//public class NodeListInspector : Editor
//{
//    private List<NodeView> m_Nodes;

//    private void OnSceneGUI ()
//    {
//        m_Nodes = target as List<NodeView>;
//        m_HandleTransform = m_Graph.transform;
//        m_HandleRotation = Tools.pivotRotation == PivotRotation.Local ? m_HandleTransform.rotation : Quaternion.identity;

//        foreach (NodeView node in m_Graph.GetNodes ())
//        {
//            ShowNode (node);
//        }
//    }

//    private void ShowNode (NodeView node)
//    {
//        Vector2 posititon = m_HandleTransform.TransformPoint (node.transform.position);
//        Quaternion rotation = node.transform.rotation;
//        EditorGUI.BeginChangeCheck ();
//        posititon = Handles.DoPositionHandle (posititon, rotation);
//        if (EditorGUI.EndChangeCheck ())
//        {
//            Undo.RecordObject (m_Graph, "Move Point");
//            EditorUtility.SetDirty (m_Graph);
//            node.transform.position = m_HandleTransform.InverseTransformPoint (posititon);
//        }
//    }
    
//    public override void OnInspectorGUI ()
//    {
//        DrawDefaultInspector ();

//        m_Graph = target as GraphView;
//        if (GUILayout.Button ("Add Node"))
//        {
//            m_Graph.AddNodes ();
//        }
//        List<NodeView> nodes = m_Graph.GetNodes ();
//        for (int index = 0; index < nodes.Count; ++index)
//        {
//            if (GUILayout.Button ("X"))
//            {
//                m_Graph.RemoveNode (index);
//            }
//        }
//    }
//}
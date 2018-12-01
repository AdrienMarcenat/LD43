using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
	private BezierCurve m_Curve;
	private Transform m_HandleTransform;
	private Quaternion m_HandleRotation;

	private const int ms_LineSteps = 10;
	private const float ms_DirectionScale = 50;

	private void OnSceneGUI ()
	{
        m_Curve = target as BezierCurve;
        m_HandleTransform = m_Curve.transform;
        m_HandleRotation = Tools.pivotRotation == PivotRotation.Local ? m_HandleTransform.rotation : Quaternion.identity;

		Vector2 p0 = ShowPoint (0);
		Vector2 p1 = ShowPoint (1);
		Vector2 p2 = ShowPoint (2);
		Vector2 p3 = ShowPoint (3);

		Handles.color = Color.gray;
		Handles.DrawLine (p0, p1);
		Handles.DrawLine (p2, p3);

		ShowDirections ();
		Handles.DrawBezier (p0, p3, p1, p2, Color.white, null, 2f);
	}

	private Vector2 ShowPoint (int index)
	{
		Vector2 point = m_HandleTransform.TransformPoint (m_Curve.m_Points [index]);
		EditorGUI.BeginChangeCheck ();
		point = Handles.DoPositionHandle (point, m_HandleRotation);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (m_Curve, "Move Point");
			EditorUtility.SetDirty (m_Curve);
            m_Curve.m_Points [index] = m_HandleTransform.InverseTransformPoint (point);
		}
		return point;
	}

	private void ShowDirections ()
	{
		Handles.color = Color.green;
		Vector2 point = m_Curve.GetPoint (0f);
		Handles.DrawLine (point, point + m_Curve.GetDirection (0f) * ms_DirectionScale);
		for (int i = 1; i <= ms_LineSteps; i++)
		{
			point = m_Curve.GetPoint (i / (float)ms_LineSteps);
			Handles.DrawLine (point, point + m_Curve.GetDirection (i / (float)ms_LineSteps) * ms_DirectionScale);
		}
	}
}
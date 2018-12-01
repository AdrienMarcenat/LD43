using UnityEngine;

public static class Bezier
{
    public static Vector2 GetPoint (Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        t = Mathf.Clamp01 (t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
        3f * oneMinusT * oneMinusT * t * p1 +
        3f * oneMinusT * t * t * p2 +
        t * t * t * p3;
    }

    public static Vector2 GetFirstDerivative (Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        t = Mathf.Clamp01 (t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
        6f * oneMinusT * t * (p2 - p1) +
        3f * t * t * (p3 - p2);
    }
}

public class BezierCurve : MonoBehaviour
{
    public Vector2[] m_Points;

    public void Reset ()
    {
        m_Points = new Vector2[] {
            new Vector2 (1f, 0f),
            new Vector2 (2f, 0f),
            new Vector2 (3f, 0f),
            new Vector2 (4f, 0f)
        };
    }

    public Vector2 GetPoint (float t)
    {
        return transform.TransformPoint (Bezier.GetPoint (m_Points[0], m_Points[1], m_Points[2], m_Points[3], t));
    }

    public Vector2 GetVelocity (float t)
    {
        return transform.TransformPoint (Bezier.GetFirstDerivative (m_Points[0], m_Points[1], m_Points[2], m_Points[3], t)) -
        transform.position;
    }

    public Vector2 GetDirection (float t)
    {
        return GetVelocity (t).normalized;
    }
}
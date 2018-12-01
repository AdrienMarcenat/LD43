using UnityEngine;

public class EnemyPath : Enemy
{
    [SerializeField] private BezierCurve m_Path;
    [SerializeField] private float m_Duration;

    private float m_Progress;

    private void Update ()
    {
        m_Progress += Time.deltaTime / m_Duration;
        if (m_Progress > 1f)
        {
            Destroy (gameObject);
        }
        Vector2 position = m_Path.GetPoint (m_Progress);
        transform.localPosition = position;
    }

    public void SetPath (BezierCurve path)
    {
        m_Path = path;
    }

    public void SetDuration (float duration)
    {
        m_Duration = duration;
    }
}


using UnityEngine;

[RequireComponent (typeof (Collider2D))]
public class LockBehaviour : MonoBehaviour
{
    [SerializeField] private string m_TargetTag;

    private Transform m_PreviousTarget;
    private Camera2D m_Camera2D;
    private bool m_IsLocked = false;


    private void Awake ()
    {
        m_Camera2D = GameObject.Find ("MainCamera").GetComponent<Camera2D> ();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == m_TargetTag && !m_IsLocked)
        {
            m_IsLocked = true;
            PushTarget (transform);
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == m_TargetTag && m_IsLocked)
        {
            m_IsLocked = false;
            PopTarget ();
        }
    }

    private void PushTarget (Transform newTarget)
    {
        m_PreviousTarget = m_Camera2D.GetTrackingTarget ();
        m_Camera2D.SetTrackingTarget (newTarget);
    }

    private void PopTarget ()
    {
        m_Camera2D.SetTrackingTarget (m_PreviousTarget);
    }

}

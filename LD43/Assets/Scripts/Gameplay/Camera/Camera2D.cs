using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    [SerializeField] private Transform m_TrackingTarget;
    [SerializeField] private float m_XOffset = 0f;
    [SerializeField] private float m_YOffset = 0f;
    [SerializeField] private float m_FollowSpeed = 5f;
    [SerializeField] private bool m_IsXLocked = false;
    [SerializeField] private bool m_IsYLocked = false;

    private Transform m_Player;
    private List<Transform> m_Lanes;

    void Awake ()
    {
        m_Player = GameObject.FindGameObjectWithTag ("Player").transform;
        m_Lanes = new List<Transform> ();
        foreach (Transform lane in GameObject.Find ("Lanes").GetComponentsInChildren<Transform> ())
        {
            m_Lanes.Add (lane);
        }
        m_Lanes.RemoveAt (0);
        m_Lanes.Sort ((Transform t1, Transform t2) => t1.position.y.CompareTo (t2.position.y));
    }

    void FixedUpdate ()
    {
        if (m_TrackingTarget == null)
            m_TrackingTarget = m_Player;

        float xTarget = m_TrackingTarget.position.x + m_XOffset;
        float yTarget = m_TrackingTarget.position.y + m_YOffset;

        float xNew = transform.position.x;
        if (!m_IsXLocked)
        {
            xNew = Mathf.Lerp (transform.position.x, xTarget, Time.deltaTime * m_FollowSpeed);
        }

        float yNew = transform.position.y;
        if (!m_IsYLocked)
        {
            yNew = Mathf.Lerp (transform.position.y, yTarget, Time.deltaTime * m_FollowSpeed);
        }
        else
        {
            foreach (Transform lane in m_Lanes)
            {
                yNew = lane.position.y;
                if (m_TrackingTarget.position.y < lane.position.y)
                {
                    break;
                }
            }
        }

        yNew = Mathf.Lerp (transform.position.y, yNew, Time.deltaTime * m_FollowSpeed);
        transform.position = new Vector3 (xNew, yNew, transform.position.z);
    }

    public Transform GetTrackingTarget ()
    {
        return m_TrackingTarget;
    }

    public void SetTrackingTarget (Transform trackingTarget)
    {
        m_TrackingTarget = trackingTarget;
    }
}

using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float m_ZoomFactor = 1.0f;
    [SerializeField] private float m_ZoomSpeed = 5.0f;

    private float m_OriginalSize = 0f;
    private Camera m_Camera;

    private void Start ()
    {
        m_Camera = GetComponent<Camera> ();
        m_OriginalSize = m_Camera.orthographicSize;
    }

    public void SetZoom (float m_ZoomFactor)
    {
        this.m_ZoomFactor = m_ZoomFactor;
        StartCoroutine (Zoom ());
    }

    IEnumerator Zoom ()
    {
        float targetSize = m_OriginalSize * m_ZoomFactor;
        while (targetSize != m_Camera.orthographicSize)
        {
            m_Camera.orthographicSize = Mathf.Lerp (m_Camera.orthographicSize,
                targetSize, Time.deltaTime * m_ZoomSpeed);
            yield return null;
        }
    }
}

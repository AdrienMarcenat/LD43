using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private int m_DamageTreshold = 20;
    [SerializeField] private float m_ShakeDuration = 0.2f;
    [SerializeField] private float m_ShakeAmount = 0.2f;

    private float m_ShakeTimer = 0;
    
    public void Shake()
    {
        StartCoroutine (ShakeRoutine ());
    }

    IEnumerator ShakeRoutine ()
    {
        float originalY = transform.localPosition.y;
        m_ShakeTimer = m_ShakeDuration;
        while (m_ShakeTimer > 0)
        {
            transform.localPosition += Random.insideUnitSphere * m_ShakeAmount;
            m_ShakeTimer -= Time.deltaTime;
            yield return null;
        }
        transform.localPosition = new Vector3 (transform.localPosition.x, originalY, transform.localPosition.z);
    }

}

using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health m_Health;

    private void Awake ()
    {
        this.RegisterAsListener (transform.parent.name, typeof (DamageGameEvent));
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener (transform.parent.name);
    }

    public void OnGameEvent (DamageGameEvent damageEvent)
    {
        float fraction = Mathf.Clamp01 (m_Health.GetCurrentHealth () / m_Health.GetTotalHealth ());
        transform.localScale = new Vector3 (fraction, transform.localScale.y, transform.localScale.z);
    }
}

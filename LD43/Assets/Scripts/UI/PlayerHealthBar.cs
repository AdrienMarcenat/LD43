using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Health m_Health;
    private Image m_Image;

    private void Awake ()
    {
        m_Image = GetComponent<Image> ();
        m_Health = GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ();
        this.RegisterAsListener ("Player", typeof (DamageGameEvent));
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Player");
    }

    public void OnGameEvent (DamageGameEvent damageEvent)
    {
        float fraction = Mathf.Clamp01 (m_Health.GetCurrentHealth () / m_Health.GetTotalHealth ());
        m_Image.fillAmount = fraction;
    }
}

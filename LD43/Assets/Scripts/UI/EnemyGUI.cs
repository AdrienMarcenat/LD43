using UnityEngine;
using System.Collections;

public class EnemyGUI : MonoBehaviour
{
	private Health m_Health;
    protected Health health
    {
        get
        {
            if (m_Health == null)
            {
                m_Health = GetComponentInParent<Health>();
            }
            return m_Health;
        }
        set
        {
            m_Health = value;
        }
    }

	[SerializeField] SpriteRenderer m_HealthBar;

    public void SetRenderer(SpriteRenderer renderer)
    {
        m_HealthBar = renderer;
    }

	void OnEnable ()
	{
        //health.SimpleDamage += UpdateUI;
	}

	void OnDisable ()
	{
        //health.SimpleDamage -= UpdateUI;
	}

	private void HealthBarEnable ()
	{
        health.enabled = true;
	}

	private void UpdateUI ()
	{
        float currentHealth = health.GetCurrentHealth ();
        float totalHealth = health.GetTotalHealth ();

		Vector3 scale = m_HealthBar.transform.localScale;
		m_HealthBar.transform.localScale = new Vector3 (currentHealth / totalHealth, scale.y, scale.z);
	}
}
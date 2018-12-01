using UnityEngine;

public class DamageGameEvent : GameEvent
{
    public DamageGameEvent(string tag, float damage) : base(tag)
    {
        m_Damage = damage;
    }

    public float GetDamage()
    {
        return m_Damage;
    }

    private float m_Damage;
}

public class GameOverGameEvent : GameEvent
{
    public GameOverGameEvent (string tag) : base (tag)
    {
    }
}

public class Health : MonoBehaviour
{
	[SerializeField] protected float m_MaxHealth;
	protected float m_CurrentHealth;
    private float m_DamageModifier;
    private bool m_Enable;

	protected void Start ()
	{
		m_CurrentHealth = m_MaxHealth;
        m_DamageModifier = 1.0f;
        m_Enable = true;
	}

    public void Heal (float value)
    {
        if (!m_Enable)
        {
            return;
        }
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + value, 0, m_MaxHealth);
    }

    public void LoseHealth (float damage)
	{
        if (!m_Enable)
        {
            return;
        }

		m_CurrentHealth = Mathf.Max (0, m_CurrentHealth - m_DamageModifier * damage);
        new DamageGameEvent (gameObject.name, damage).Push();

		CheckIfGameOver ();
	}

	public float GetCurrentHealth ()
	{
		return m_CurrentHealth;
	}

    public void SetMaxHealth (float value)
    {
        m_MaxHealth = value;
    }

    public float GetTotalHealth ()
	{
		return m_MaxHealth;
	}

	private void CheckIfGameOver ()
	{
		if (m_CurrentHealth <= 0)
		{
			new GameOverGameEvent (gameObject.name).Push ();
		}
	}

	public void Enable (bool enable)
	{
		m_Enable = enable;
	}

	public void SetDamageModifier (float modifier)
	{
		m_DamageModifier = modifier;
	}
}


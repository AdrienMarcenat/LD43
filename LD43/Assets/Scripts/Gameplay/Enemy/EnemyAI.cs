using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float m_AimedPrecision;
    [SerializeField] protected float m_FireSalveNumber;
    [SerializeField] protected float m_FireRate;
    [SerializeField] protected float m_SizeModifier;
    [SerializeField] protected Transform m_ShootDirection;

    protected WeaponManager m_WeaponManager;
    protected float m_FireDelay;

    protected virtual void Fire ()
    {

    }

    void Start ()
    {
        m_WeaponManager = GetComponent<WeaponManager> ();
        m_FireDelay = m_FireRate;
    }

    void Update ()
    {
        if (m_FireDelay < m_FireRate)
        {
            m_FireDelay += Time.deltaTime;
        }
        else
        {
            Fire ();
            m_FireDelay = 0;
        }
    }

    public void SetFireParameters (float rate, float salveNumber, float sizeModifier, float precision)
    {
        m_FireRate = rate;
        m_FireSalveNumber = salveNumber;
        m_SizeModifier = sizeModifier;
        m_AimedPrecision = precision;
    }

    public void SetShootDirection (Transform shootDirection)
    {
        m_ShootDirection = shootDirection;
    }
}


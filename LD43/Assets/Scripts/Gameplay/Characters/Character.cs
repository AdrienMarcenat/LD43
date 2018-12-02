using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAction
{
    Attack,
    Defense,
    Heal,
    Protect,
    Bound
}

public class Character : MonoBehaviour
{
    protected CharacterModel m_Model;
    protected Health m_Health;
    protected SpriteRenderer m_Sprite;
    protected List<EAction> m_BattleActions;
    protected int m_Resistance = 0;
    protected bool m_Protected = false;
    protected bool m_Bound = false;


	// Use this for initialization
	protected void Start ()
    {
        m_Health = GetComponent<Health> ();
		
	}

    private void OnDestroy ()
    {
    }

    // Update is called once per frame
    protected void Update ()
    {
		
	}

    IEnumerator HitRoutine(float damage)
    {
        // TODO: Define damage routine
        yield return null;
    }

    public void OnGameEvent (DamageGameEvent damageEvent)
    {
        StartCoroutine (HitRoutine (damageEvent.GetDamage ()));
    }

    public List<EAction> GetBattleActions ()
    {
        return m_BattleActions;
    }

    public bool TakeDamage (int damage)
    {
        // If character is protected
        if (m_Protected)
        {
            return false;
        }
        // TODO: Implement taking damage, return true if character is dead, false if not
        return false;
    }

    public void Heal (int heal)
    {
        // TODO: Implement healing
    }

    public void Resistance (int resistance)
    {
        m_Resistance = resistance;
    }

    public void Protected ()
    {
        m_Protected = true;
    }

    public void Bound ()
    {
        m_Bound = true;
    }

    public void ResetCondition ()
    {
        m_Resistance = 0;
        m_Protected = false;
        m_Bound = false;
    }

    public int GetStrength ()
    {
        return m_Model.GetStrength ();
    }

    public int GetMagic ()
    {
        return m_Model.GetMagic ();
    }
}

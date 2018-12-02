using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAction
{
    Attack,
    Defense,
    Heal
}

public class Character : MonoBehaviour
{
    protected CharacterModel m_Model;
    protected Health m_Health;
    protected SpriteRenderer m_Sprite;
    protected List<EAction> m_BattleActions;
    protected int m_Resistance = 0;


	// Use this for initialization
	protected void Start ()
    {
        m_Health = GetComponent<Health> ();

        this.RegisterAsListener (gameObject.name, typeof (DamageGameEvent));
		
	}

    private void OnDestroy ()
    {
        this.UnregisterAsListener (gameObject.name);
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
        // Implement taking damage, return true if character is dead, false if not
        return false;
    }

    public void Heal (int heal)
    {
        // Implement healing
    }

    public void Resistance (int resistance)
    {
        m_Resistance = resistance;
    }

    public void ResetResistance ()
    {
        m_Resistance = 0;
    }
}

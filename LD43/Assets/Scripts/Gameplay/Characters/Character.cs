using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterModel m_Model;
    protected Health m_Health;
    protected SpriteRenderer m_Sprite;


	// Use this for initialization
	protected void Start ()
    {
        m_Health = GetComponent<Health> ();

        this.RegisterAsListener (gameObject.name, typeof (DamageGameEvent));
		
	}
	
	// Update is called once per frame
	protected void Update ()
    {
		
	}

    IEnumerator HitRoutine(float damage)
    {
        // TODO: Define damage routine
    }

    public void OnGameEvent (DamageGameEvent damageEvent)
    {
        StartCoroutine (HitRoutine (damageEvent.GetDamage ()));
    }
}

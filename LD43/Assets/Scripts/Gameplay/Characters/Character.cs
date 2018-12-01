using System.Collections;
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
}

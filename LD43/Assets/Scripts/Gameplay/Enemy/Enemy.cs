using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int m_Type;
    [SerializeField] protected float m_PlayerDamageOnCollision;
    [SerializeField] private AudioClip m_Sound;
    [SerializeField] private float m_HitColorTime;

    protected Transform m_Target;
    protected Health m_Health;
    protected Animator m_Animator;
    protected SpriteRenderer m_Sprite;
    protected Color m_InitialColor;

    protected void Awake ()
    {
        m_Health = GetComponent<Health> ();
        m_Sprite = GetComponentInChildren<SpriteRenderer> ();
        m_InitialColor = m_Sprite.color;

        this.RegisterAsListener (gameObject.name, typeof (GameOverGameEvent), typeof (DamageGameEvent));
    }

    IEnumerator HitRoutine (float damage)
    {
        m_Sprite.color = Color.Lerp (Color.blue, Color.red, damage / 100);
        yield return new WaitForSeconds (m_HitColorTime);
        m_Sprite.color = m_InitialColor;
    }

    public void OnGameEvent (DamageGameEvent damageEvent)
    {
        StartCoroutine (HitRoutine (damageEvent.GetDamage ()));
    }

    public void OnGameEvent (GameOverGameEvent gameOverEvent)
    {
        GetComponent<BoxCollider2D> ().enabled = false;
        Destroy (gameObject, 1);
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener (gameObject.name);
    }

    private void OnCollisionStay2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Health playerHealth = other.gameObject.GetComponent<Health> ();
            playerHealth.LoseHealth (m_PlayerDamageOnCollision);
        }
    }
}
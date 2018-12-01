using UnityEngine;
using System.Collections;

public class PlayerInvicibleState : HSMState
{
    [SerializeField] private float m_InvulnerabilitySeconds;
    [SerializeField] private float m_BlinkingRate;
    private Health m_Health;
    private SpriteRenderer m_Sprite;
    private float m_InvulnerabilitySecondsDelay;
    
    public override void OnEnter ()
    {
        m_Health.Enable (false);
        //StartCoroutine (InvulnerabilityRoutine ());
    }

    public override void OnExit ()
    {
        m_Sprite.enabled = true;
        m_Health.Enable (true);
    }

    IEnumerator InvulnerabilityRoutine ()
    {
        m_InvulnerabilitySecondsDelay = m_InvulnerabilitySeconds;
        while (m_InvulnerabilitySecondsDelay > 0)
        {
            m_InvulnerabilitySecondsDelay -= Time.deltaTime + m_BlinkingRate;
            m_Sprite.enabled = !m_Sprite.enabled;
            yield return new WaitForSeconds (m_BlinkingRate);
        }
        ChangeNextTransition (HSMTransition.EType.Exit);
    }
}


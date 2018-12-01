using UnityEngine;

public class PlayerNormalState : HSMState
{
	private Health m_Health;
	private MovingObject m_Body;
	private WeaponManager m_WeaponManager;
    
	public override void OnEnter ()
	{
        this.RegisterAsListener ("Player", typeof (GameOverGameEvent), typeof (DamageGameEvent));
	}

	public override void OnExit ()
    {
        this.UnregisterAsListener ("Player");
    }

    public void OneGameEvent (GameOverGameEvent gameOverEvent)
	{
        ChangeNextTransition (HSMTransition.EType.Clear, typeof(PlayerGameOverState));
	}

	public void OneGameEvent (DamageGameEvent damageEvent)
    {
        ChangeNextTransition (HSMTransition.EType.Child, typeof (PlayerInvicibleState));
    }
}


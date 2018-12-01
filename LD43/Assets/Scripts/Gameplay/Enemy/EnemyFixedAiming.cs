
public class EnemyFixedAiming : EnemyAI
{
    protected override void Fire ()
    {
        m_WeaponManager.AddFireCommand (0, m_FireSalveNumber, m_SizeModifier, m_ShootDirection.localPosition);
    }
}


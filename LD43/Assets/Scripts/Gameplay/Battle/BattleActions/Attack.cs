public class Attack : BattleAction
{
    private int m_Strength = 0;

    public Attack (int strength, Character source, Character target)
        : base(source, target, EAction.Attack)
    {
        m_Strength = strength;
    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Attack (m_Strength);
    }
}

using System.Collections.Generic;

public class Heal : BattleAction
{
    private int m_Heal = 0;

    public Heal(int heal, Character source, Character target)
        : base (source, target, EAction.Heal)
    {
        m_Heal = heal;
    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Heal (m_Heal);
    }
}

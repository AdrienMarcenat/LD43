using System.Collections.Generic;

public class Defense : BattleAction
{
    private int m_Protection = 0;

    public Defense (int protection, Character source, Character target)
        : base (source, target, EAction.Defense)
    {
        m_Protection = protection;
    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Defense (m_Protection);
    }
}

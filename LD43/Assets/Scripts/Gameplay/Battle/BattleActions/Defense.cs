using System.Collections.Generic;

public class Defense : BattleAction
{
    private int m_Protection = 0;

    public Defense (int protection)
    {
        m_Protection = protection;
    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Defense (m_Protection);
    }
}

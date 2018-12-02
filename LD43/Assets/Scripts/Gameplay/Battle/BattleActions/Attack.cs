using System.Collections.Generic;

public class Attack : BattleAction
{
    private int m_Strength = 0;

    public Attack (int strength)
    {
        m_Strength = strength;
    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Attack (m_Strength);
    }
}

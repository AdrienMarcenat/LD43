using System.Collections.Generic;

public class Attack : BattleAction
{
    private int m_Strength = 0;
    private List<Character> m_Targets = null;

    public Attack (int strength, List<Character> targets)
    {
        m_Strength = strength;
        m_Targets = targets;
    }

    public override void ApplyAction ()
    {
        // TODO: Apply the Attack
    }
}

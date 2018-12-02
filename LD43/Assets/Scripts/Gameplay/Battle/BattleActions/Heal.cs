using System.Collections.Generic;

public class Heal : BattleAction
{
    private int m_Heal = 0;
    private List<Character> m_Targets = null;

    public Heal(int heal, List<Character> targets)
    {
        m_Heal = heal;
        m_Targets = targets;
    }

    public override void ApplyAction ()
    {
        // TODO: Apply the Attack
    }
}

using System.Collections.Generic;

public class Defense : BattleAction
{
    private int m_Protection = 0;
    private List<Character> m_Targets = null;

    public Defense (int protection, List<Character> targets)
    {
        m_Protection = protection;
        m_Targets = targets;
    }

    public override void ApplyAction ()
    {
        // TODO: Apply the Attack
    }
}

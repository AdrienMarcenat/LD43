public enum EBattleAction
{
    ChooseAction,
    Effect,
    Nothing,
    Success,
    Failure
}

public class BattleEvent : GameEvent
{
    public BattleEvent (EBattleAction action) : base ("Battle")
    {
        m_Action = action;
    }

    public EBattleAction GetAction ()
    {
        return m_Action;
    }

    private EBattleAction m_Action;
}

public class BattleHSM : HSM
{
    public BattleHSM ()
        : base (new BattleStandbyState (),
                new BattleActionState (),
                new BattleAftermathState ()
        )
    {

    }
}
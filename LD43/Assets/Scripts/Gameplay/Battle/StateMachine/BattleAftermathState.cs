public class BattleAftermathState : HSMState
{
    public override void OnEnter ()
    {
        this.RegisterAsListener ("Battle", typeof (BattleEvent));
        BattleManagerProxy.Get ().TurnEnd ();
    }

    public void OnGameEvent (BattleEvent battleEvent)
    {
        switch (battleEvent.GetAction ())
        {
            case EBattleAction.Nothing:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (BattleStandbyState));
                BattleManagerProxy.Get ().ResetCondition ();
                BattleManagerProxy.Get ().NextPlayerTurn ();
                break;
            case EBattleAction.Success:
                // TODO: Determine what happens
                break;
            case EBattleAction.Failure:
                // TODO: Determine what happens
                break;
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Battle");
    }
}
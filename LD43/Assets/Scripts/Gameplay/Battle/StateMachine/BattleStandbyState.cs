public class BattleStandbyState : HSMState
{
    public override void OnEnter ()
    {
        BattleManagerProxy.Get ().SetupTurn ();
        this.RegisterAsListener ("Battle", typeof (BattleEvent));
    }

    public void OnGameEvent (BattleEvent battleEvent)
    {
        if (battleEvent.GetAction () == EBattleAction.ChooseAction)
        {
            ChangeNextTransition (HSMTransition.EType.Clear, typeof (BattleActionState));
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Battle");
    }
}
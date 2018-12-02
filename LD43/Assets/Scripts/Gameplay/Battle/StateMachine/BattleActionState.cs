public class BattleActionState : HSMState
{
    public override void OnEnter ()
    {
        this.RegisterAsListener ("Battle", typeof (BattleEvent));
        BattleManagerProxy.Get ().ApplyActions ();
    }

    public void OnGameEvent (BattleEvent battleEvent)
    {
        if (battleEvent.GetAction () == EBattleAction.Effect)
        {
            ChangeNextTransition (HSMTransition.EType.Clear, typeof (BattleAftermathState));
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Battle");
    }
}
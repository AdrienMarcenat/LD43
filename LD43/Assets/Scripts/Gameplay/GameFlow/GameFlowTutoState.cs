public class GameFlowTutoState : HSMState
{

    public override void OnEnter ()
    {
        // TODO Load Tuto
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        if (flowEvent.GetAction () == EGameFlowAction.LoadGame)
        {
            ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowLevelState));
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
    }
}

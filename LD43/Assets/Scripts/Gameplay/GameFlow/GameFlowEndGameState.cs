public class GameFlowEndGameState : HSMState
{

    public override void OnEnter ()
    {
        // TODO: Launch End game
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        if (flowEvent.GetAction () == EGameFlowAction.Menu)
        {
            ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowMenuState));
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
    }
}


public class GameFlowEdgeState : HSMState
{
    public override void OnEnter ()
    {
        LevelManagerProxy.Get ().LoadScene (1);
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
        this.RegisterAsListener ("Player", typeof (GameOverGameEvent), typeof (PlayerInputGameEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.SuccessEdge:
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowNormalState));
                break;
            case EGameFlowAction.FailureEdge:
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowNormalState));
                break;
        }
    }

    public void OnGameEvent (GameOverGameEvent gameOver)
    {
        ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowGameOverState));
    }

    public void OnGameEvent (PlayerInputGameEvent inputEvent)
    {
        if (inputEvent.GetInput () == "Pause" && inputEvent.GetInputState () == EInputState.Down && !UpdaterProxy.Get ().IsPaused ())
        {
            ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowPauseState));
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        this.UnregisterAsListener ("Player");
    }
}
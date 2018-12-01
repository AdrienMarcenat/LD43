
public class GameFlowNormalState : HSMState
{
    public override void OnEnter ()
    {
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
        this.RegisterAsListener ("Player", typeof (PlayerInputGameEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.EnterNode:
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowNodeState));
                break;
            case EGameFlowAction.EnterEdge:
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowEdgeState));
                break;
            case EGameFlowAction.TeamManagement:
                ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowTeamManagementState));
                break;
        }
    }

    public void OnGameEvent (PlayerInputGameEvent inputEvent)
    {
        if (inputEvent.GetInput () == "Pause" && inputEvent.GetInputState() == EInputState.Down && !UpdaterProxy.Get().IsPaused())
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
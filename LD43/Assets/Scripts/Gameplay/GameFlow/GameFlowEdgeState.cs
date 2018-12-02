
public class GameFlowEdgeState : HSMState
{
    public override void OnEnter ()
    {
        // TODO: Edge enter logic
        UpdaterProxy.Get ().SetPause (true);
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
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

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        this.UnregisterAsListener ("Player");
        UpdaterProxy.Get ().SetPause (false);
    }
}
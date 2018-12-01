
public class GameFlowNodeState : HSMState
{
    public override void OnEnter ()
    {
        // TODO: Node enter logic
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.LeaveNode:
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowNormalState));
                break;
            case EGameFlowAction.LevelWon:
                if (!LevelManagerProxy.Get ().IsLastLevel ())
                {
                    LevelManagerProxy.Get ().NextLevel ();
                    ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowLevelState));
                }
                else
                {
                    ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowEndGameState));
                }
                break;
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        this.UnregisterAsListener ("Player");
    }
}
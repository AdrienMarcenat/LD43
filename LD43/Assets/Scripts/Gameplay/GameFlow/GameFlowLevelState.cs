
public class GameFlowLevelState : HSMState
{
    public override void OnEnter ()
    {
        LevelManagerProxy.Get ().LoadLevel();
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));

        ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowNormalState));
    }
    
    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
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
            case EGameFlowAction.GameOver:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowGameOverState));
                break;
        }
    }

    public void OnGameEvent (GameOverGameEvent gameOver)
    {
        ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowGameOverState));
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
    }
}
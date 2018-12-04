
public class GameFlowLevelState : HSMState
{
    private bool m_HasEnded;

    public override void OnEnter ()
    {
        m_HasEnded = false;
        LevelManagerProxy.Get ().LoadLevel();
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));

        ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowNormalState));
    }
    
    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.EndDialogue:
                if (m_HasEnded)
                {
                    if (!LevelManagerProxy.Get ().IsLastLevel ())
                    {
                        LevelManagerProxy.Get ().NextLevel ();
                        ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowLevelState));
                    }
                    else
                    {
                        ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowEndGameState));
                    }
                }
                break;
            case EGameFlowAction.GameOver:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowGameOverState));
                break;
            case EGameFlowAction.LevelWon:
                m_HasEnded = true;
                break;
            case EGameFlowAction.Menu:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowMenuState));
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
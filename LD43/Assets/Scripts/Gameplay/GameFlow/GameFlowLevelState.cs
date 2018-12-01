
public class GameFlowLevelState : HSMState
{
    public override void OnEnter ()
    {
        LevelManagerProxy.Get ().LoadLevel();
        this.RegisterAsListener ("Player", typeof (PlayerInputGameEvent), typeof (GameOverGameEvent));
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));

        ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowNormalState));
    }

    public void OnGameEvent (PlayerInputGameEvent inputEvent)
    {
        if (inputEvent.GetInput () == "Pause" && inputEvent.GetInputState() == EInputState.Down && !UpdaterProxy.Get().IsPaused())
        {
            ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowPauseState));
        }
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
        }
    }

    public void OnGameEvent (GameOverGameEvent gameOver)
    {
        ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowGameOverState));
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        this.UnregisterAsListener ("Player");
    }
}
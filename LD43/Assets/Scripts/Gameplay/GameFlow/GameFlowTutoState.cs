public class GameFlowTutoState : HSMState
{

    public override void OnEnter ()
    {
        // TODO Create the Tuto scene
        LevelManagerProxy.Get ().LoadScene ("Scenes/Levels/Tuto");
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
        this.RegisterAsListener ("Player", typeof (PlayerInputGameEvent));
    }

    public void OnGameEvent (PlayerInputGameEvent inputEvent)
    {
        if (inputEvent.GetInput () == "Pause" && inputEvent.GetInputState () == EInputState.Down && !UpdaterProxy.Get ().IsPaused ())
        {
            ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowPauseState));
        }
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

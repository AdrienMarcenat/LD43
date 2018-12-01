public class GameFlowMenuState : HSMState
{
    public override void OnEnter ()
    {
        LevelManagerProxy.Get ().LoadScene ("Scenes/MainMenu");
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.Start:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowTutoState));
                break;
            case EGameFlowAction.Quit:
                // Quit game
                break;
        }

    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
    }
}
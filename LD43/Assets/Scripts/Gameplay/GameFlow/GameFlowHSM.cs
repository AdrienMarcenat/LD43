public enum EGameFlowAction
{
    Start,
    LoadGame,
    EndGame,
    GameOver,
    Pause,
    Exit,
    Resume,
    Menu,
    LoadLevel,
    EnterLevel,
    NextLevel,
    EndLevel,
    Retry,
    Quit,
    EnterNode,
    LeaveNode,
    LevelWon,
    EnterEdge,
    SuccessEdge,
    FailureEdge,
    TeamManagement,
    TeamManagementBack,

    // Next ones are useful to not have issues with other files
    StartDialogue,
    EndDialogue,
    EndLevelPanel,
    LevelSelection
}

public class GameFlowEvent : GameEvent
{
    public GameFlowEvent (EGameFlowAction action) : base ("Game")
    {
        m_Action = action;
    }

    public EGameFlowAction GetAction ()
    {
        return m_Action;
    }

    private EGameFlowAction m_Action;
}

public class GameFlowHSM : HSM
{
    public GameFlowHSM ()
        : base (new GameFlowMenuState ()
              , new GameFlowLevelSelectionState()
              , new GameFlowLevelState ()
              , new GameFlowDialogueState()
              , new GameFlowPauseState ()
              , new GameFlowEndLevelState ()
        )
    {
        
    }
}
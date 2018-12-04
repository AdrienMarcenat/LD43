public enum EGameFlowAction
{
    Start,
    LoadGame,
    EndGame,
    GameOver,
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
    TeamManagement,
    TeamManagementBack,
    StartDialogue,
    EndDialogue,
    EndGamePanel
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
              , new GameFlowLevelState ()
              , new GameFlowGameOverState ()
              , new GameFlowNodeState ()
              , new GameFlowEdgeState ()
              , new GameFlowEndGameState ()
              , new GameFlowNormalState ()
              , new GameFlowDialogueState ()
        )
    {
        
    }
}
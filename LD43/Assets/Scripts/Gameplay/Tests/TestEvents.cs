using UnityEngine;

public class TestEvents : MonoBehaviour
{

	public void StartEvent ()
    {
        new GameFlowEvent (EGameFlowAction.Start).Push ();
    }

    public void LoadGameEvent ()
    {
        new GameFlowEvent (EGameFlowAction.LoadGame).Push ();
    }

    public void EndGameEvent ()
    {
        new GameFlowEvent (EGameFlowAction.EndGame).Push ();
    }

    public void GameOverEvent ()
    {
        new GameOverGameEvent ("Player").Push ();
    }

    public void PauseEvent ()
    {
        new GameFlowEvent (EGameFlowAction.Pause).Push ();
    }

    public void ExitEvent ()
    {
        new GameFlowEvent (EGameFlowAction.Exit).Push ();
    }

    public void ResumeEvent ()
    {
        new GameFlowEvent (EGameFlowAction.Resume).Push ();
    }

    public void MenuEvent ()
    {
        new GameFlowEvent (EGameFlowAction.Menu).Push ();
    }

    public void LoadLevelEvent ()
    {
        new GameFlowEvent (EGameFlowAction.LoadLevel).Push ();
    }

    public void EnterLevelEvent ()
    {
        new GameFlowEvent (EGameFlowAction.EnterLevel).Push ();
    }

    public void NextLevelEvent ()
    {
        new GameFlowEvent (EGameFlowAction.NextLevel).Push ();
    }

    public void EndLevelEvent ()
    {
        new GameFlowEvent (EGameFlowAction.EndLevel).Push ();
    }

    public void RetryEvent ()
    {
        new GameFlowEvent (EGameFlowAction.Retry).Push ();
    }

    public void QuitEvent ()
    {
        new GameFlowEvent (EGameFlowAction.Quit).Push ();
    }

    public void EnterNodeEvent ()
    {
        new GameFlowEvent (EGameFlowAction.EnterNode).Push ();
    }

    public void LeaveNodeEvent ()
    {
        new GameFlowEvent (EGameFlowAction.LeaveNode).Push ();
    }

    public void LevelWonEvent ()
    {
        new GameFlowEvent (EGameFlowAction.LevelWon).Push ();
    }

    public void EnterEdgeEvent ()
    {
        new GameFlowEvent (EGameFlowAction.EnterEdge).Push ();
    }

    public void SuccessEdgeEvent ()
    {
        new GameFlowEvent (EGameFlowAction.SuccessEdge).Push ();
    }

    public void TeamManagementEvent ()
    {
        new GameFlowEvent (EGameFlowAction.TeamManagement).Push ();
    }

    public void TeamManagementBackEvent ()
    {
        new GameFlowEvent (EGameFlowAction.TeamManagementBack).Push ();
    }
}

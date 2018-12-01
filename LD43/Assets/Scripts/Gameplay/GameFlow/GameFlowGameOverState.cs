using UnityEngine;

public class GameFlowGameOverState : HSMState
{
    public override void OnEnter ()
    {
        UpdaterProxy.Get ().SetPause (true);
        Time.timeScale = 0;
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.Quit:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowMenuState));
                break;
            case EGameFlowAction.Retry:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowLevelState));
                break;
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        Time.timeScale = 1;
        UpdaterProxy.Get ().SetPause (false);
    }
}
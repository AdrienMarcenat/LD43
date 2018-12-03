using UnityEngine;

public class GameFlowGameOverState : HSMState
{
    public override void OnEnter ()
    {
        UpdaterProxy.Get ().SetPause (true);
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
        new GameOverGameEvent ("Player").Push ();
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.Menu:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowMenuState));
                break;
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        UpdaterProxy.Get ().SetPause (false);
    }
}
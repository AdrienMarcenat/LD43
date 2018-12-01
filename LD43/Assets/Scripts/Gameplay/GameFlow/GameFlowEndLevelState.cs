using UnityEngine;

public class GameFlowEndLevelState : HSMState
{
    public override void OnEnter ()
    {
        UpdaterProxy.Get ().SetPause (true);
        LevelManagerProxy.Get ().OnLevelEnd ();
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.StartDialogue:
                ChangeNextTransition (HSMTransition.EType.Child, typeof (GameFlowDialogueState));
                break;
            case EGameFlowAction.EndDialogue:
                new GameFlowEvent (EGameFlowAction.EndLevelPanel).Push ();
                break;
            case EGameFlowAction.LevelSelection:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowLevelSelectionState));
                break;
            case EGameFlowAction.Menu:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowMenuState));
                break;
            case EGameFlowAction.Retry:
                ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowLevelState));
                break;
            case EGameFlowAction.NextLevel:
                if (!LevelManagerProxy.Get ().IsLastLevel ())
                {
                    LevelManagerProxy.Get ().NextLevel ();
                    ChangeNextTransition (HSMTransition.EType.Clear, typeof (GameFlowLevelState));
                }
                break;
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        UpdaterProxy.Get ().SetPause (false);
    }
}
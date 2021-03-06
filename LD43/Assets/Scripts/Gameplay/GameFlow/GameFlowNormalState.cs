﻿
public class GameFlowNormalState : HSMState
{
    public override void OnEnter ()
    {
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.EnterNode:
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowNodeState));
                break;
            case EGameFlowAction.EnterEdge:
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowEdgeState));
                break;
            case EGameFlowAction.StartDialogue:
                ChangeNextTransition (HSMTransition.EType.Top, typeof (GameFlowDialogueState));
                break;
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        this.UnregisterAsListener ("Player");
    }
}
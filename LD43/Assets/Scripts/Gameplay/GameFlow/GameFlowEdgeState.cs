
using UnityEngine;

public class GameFlowEdgeState : HSMState
{
    private OverworldPlayerController m_Player;

    public override void OnEnter ()
    {
        // TODO: Edge enter logic
        UpdaterProxy.Get ().SetPause (true);
        m_Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<OverworldPlayerController> ();
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.SuccessEdge:
                m_Player.OnEdge (null, false);
                ChangeNextTransition (HSMTransition.EType.Siblings, typeof (GameFlowNormalState));
                break;
        }
    }

    public override void OnExit ()
    {
        this.UnregisterAsListener ("Game");
        this.UnregisterAsListener ("Player");
        UpdaterProxy.Get ().SetPause (false);
    }
}

public class GameFlowTeamManagementState : HSMState
{
    public override void OnEnter ()
    {
        // TODO: Team management enter logic
        UpdaterProxy.Get ().SetPause (true);
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent flowEvent)
    {
        switch (flowEvent.GetAction ())
        {
            case EGameFlowAction.TeamManagementBack:
                ChangeNextTransition (HSMTransition.EType.Exit);
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
using UnityEngine;
using System.Collections;

public class PlayerHSM : HSM
{
    public PlayerHSM ()
        : base (new GameFlowNormalState ()
              , new GameFlowPauseState ()
        )
    {
        Start (typeof (GameFlowNormalState));
    }
}


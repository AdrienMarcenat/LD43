﻿using UnityEngine;
using UnityEngine.UI;

public class OnDiversionEvent : GameEvent
{
    public OnDiversionEvent () : base ("Game")
    {

    }
}

public class OnCharacterDiversionEvent : GameEvent
{
    public OnCharacterDiversionEvent () : base ("Game")
    {

    }
}

public class DiversionPanel : MonoBehaviour
{
	void Awake ()
    {
        this.RegisterAsListener ("Game", typeof (OnDiversionEvent), typeof (OnCharacterDiversionEvent));
        gameObject.SetActive (false);
	}
	
    public void OnGameEvent (OnDiversionEvent diversionEvent)
    {
        gameObject.SetActive (true);
        TeamManagerProxy.Get ().WaitForDiversion ();
    }

    public void OnGameEvent (OnCharacterDiversionEvent characterDiversionEvent)
    {
        gameObject.SetActive (false);
        new GameFlowEvent (EGameFlowAction.SuccessEdge).Push();
    }

    public void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }
}

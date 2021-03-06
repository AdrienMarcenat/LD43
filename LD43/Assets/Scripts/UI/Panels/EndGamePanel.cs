﻿using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    private void Awake ()
    {
        gameObject.SetActive (false);
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent gameFlowEvent)
    {
        if(gameFlowEvent.GetAction() == EGameFlowAction.EndGamePanel)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }
}


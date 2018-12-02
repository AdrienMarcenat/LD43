﻿using UnityEngine;
using UnityEngine.UI;

public class EdgePanel : MonoBehaviour
{
    private EdgeView m_CurrentEdge;
    private OverworldPlayerController m_Player;
    [SerializeField] private Button m_ChoosePathButton;

    private void Awake ()
    {
        m_Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<OverworldPlayerController> ();
        this.RegisterAsListener ("Game", typeof (OnEdgeClick));
        gameObject.SetActive (false);
    }
    
    public void OnGameEvent (OnEdgeClick edgeEvent)
    {
        gameObject.SetActive (true);
        m_CurrentEdge = edgeEvent.GetEdge ();
        m_ChoosePathButton.interactable = m_Player.CanMoveToEdge(m_CurrentEdge);
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }

    public void Cancel()
    {
        m_CurrentEdge = null;
        gameObject.SetActive (false);
    }

    public void ChoosePath()
    {
        new OnEdgeGameEvent ("Player", m_CurrentEdge, true).Push ();
        gameObject.SetActive (false);
    }
}


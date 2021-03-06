﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpdateUIGameEvent : GameEvent
{
    public UpdateUIGameEvent () : base ("UI")
    {}
}

public class TeamPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Players;
    [SerializeField] private GameObject m_WarningPanel;
    private List<CharacterModel> m_Models;
    private int m_PendingRemoveIndex;

    // Use this for initialization
    void Awake ()
    {
        this.RegisterAsListener ("UI", typeof (UpdateUIGameEvent));
        this.RegisterAsListener ("Player", typeof (GameOverGameEvent));
        UpdateUI ();
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("UI");
        this.UnregisterAsListener ("Player");
    }

    public void OnGameEvent(UpdateUIGameEvent uiEvent)
    {
        UpdateUI ();
    }

    public void OnGameEvent (GameOverGameEvent uiEvent)
    {
        gameObject.SetActive (false);
    }

    private void UpdateUI ()
    {
        m_Models = new List<CharacterModel> ();
        int index = 0;
        foreach (CharacterModel model in TeamManagerProxy.Get ().GetSortedTeam ())
        {
            m_Models.Add (model);
            m_Players[index].SetActive (true);
            m_Players[index].GetComponentInChildren<Image>().sprite = RessourceManager.LoadSprite ("Models/" + model.GetClass ().ToString (), 0);
            index++;
        }
        for(int i = index; i < m_Players.Count; ++i)
        {
            m_Players[i].SetActive (false);
        }
    }

    public void RemoveFromTeam(int index)
    {
        m_WarningPanel.SetActive (true);
        m_PendingRemoveIndex = index;
    }

    public void ConfirmRemove()
    {
        m_WarningPanel.SetActive (false);
        TeamManagerProxy.Get ().RemoveCharacterWithDialogue (m_Models[m_PendingRemoveIndex].GetName());
    }

    public void CancelRemove ()
    {
        m_WarningPanel.SetActive (false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class OnBattleGameEvent : GameEvent
{
    public OnBattleGameEvent(bool enter) : base("Game")
    {
        m_Enter = enter;
    }

    public bool m_Enter;
}

public class BattlePanel : MonoBehaviour
{
    [SerializeField] List<Image> m_PlayerThumbnails;
    [SerializeField] List<Image> m_EnemiesThumbnails;
    [SerializeField] Button m_AttackButton;
    [SerializeField] Button m_DefendButton;
    [SerializeField] Button m_CapacityButton;

    private List<Character> m_Team;
    private Character m_CurrentPlayer;
    private int m_CurrentIndex;

    private void Awake ()
    {
        this.RegisterAsListener ("Game", typeof (OnBattleGameEvent));
        gameObject.SetActive (false);
    }

    public void OnGameEvent (OnBattleGameEvent battleEvent)
    {
        if (battleEvent.m_Enter)
        {
            gameObject.SetActive (true);
            StartCoroutine (Init ());
        }
        else
        {
            StartCoroutine(Reset ());
        }
    }

    IEnumerator Init ()
    {
        yield return null;
        m_CurrentIndex = 0;
        m_Team = new List<Character> ();
        int index = 0;
        foreach (CharacterModel model in TeamManagerProxy.Get ().GetTeam ().Values)
        {
            m_Team.Add (new Character(model));
            m_PlayerThumbnails[index].sprite = RessourceManager.LoadSprite ("Models/" + model.GetClass().ToString(), 0);
            index++;
        }
        m_CurrentPlayer = m_Team[0];
        SetCurrentPlayerUI ();
        BattleManagerProxy.Get ().Init (m_Team);
    }

    IEnumerator Reset ()
    {
        yield return null;
        m_CurrentIndex = 0;
        BattleManagerProxy.Get ().Shutdown ();
        new GameFlowEvent (EGameFlowAction.SuccessEdge).Push ();
        gameObject.SetActive (false);
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }
    
    public void Attack()
    {
        BattleManagerProxy.Get ().AddAction (EAction.Attack, m_CurrentPlayer);
        NextPlayer ();
    }

    public void Defend ()
    {
        BattleManagerProxy.Get ().AddAction (EAction.Defense, m_CurrentPlayer);
        NextPlayer ();
    }

    public void UseCapacity ()
    {
        BattleManagerProxy.Get ().AddAction (EAction.Defense, m_CurrentPlayer);
        NextPlayer ();
    }

    private void NextPlayer()
    {
        m_PlayerThumbnails[m_CurrentIndex].color = Color.white;
        m_CurrentIndex++;
        if (m_CurrentIndex == m_Team.Count)
        {
            new BattleEvent (EBattleAction.ChooseAction).Push ();
            m_CurrentIndex = 0;
        }
        else
        {
            m_CurrentPlayer = m_Team[m_CurrentIndex];
            if(m_CurrentPlayer.IsBound ())
            {
                m_PlayerThumbnails[m_CurrentIndex].color = Color.blue;
                NextPlayer ();
            }
            if (m_CurrentPlayer.IsDead ())
            {
                m_PlayerThumbnails[m_CurrentIndex].color = Color.black;
                NextPlayer ();
            }
            SetCurrentPlayerUI ();
        }
    }

    private void SetCurrentPlayerUI ()
    {
        m_PlayerThumbnails[m_CurrentIndex].color = Color.green;
        m_CapacityButton.GetComponentInChildren<Text> ().text = m_CurrentPlayer.GetModel ().GetCapacity ().ToString ();
    }
}

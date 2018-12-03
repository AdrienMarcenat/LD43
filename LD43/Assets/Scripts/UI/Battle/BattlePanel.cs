using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class OnBattleGameEvent : GameEvent
{
    public OnBattleGameEvent(bool enter, EdgeResource resource) : base("Game")
    {
        m_Enter = enter;
        m_Resource = resource;
    }

    public bool m_Enter;
    public EdgeResource m_Resource;
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
            Reset ();
        }
    }

    IEnumerator Init ()
    {
        yield return null;
        m_CurrentIndex = 0;
        m_Team = new List<Character> ();
        foreach (CharacterModel model in TeamManagerProxy.Get ().GetTeam ().Values)
        {
            m_Team.Add (new Character(model));
        }
        m_CurrentPlayer = m_Team[0];
        BattleManagerProxy.Get ().Init (m_Team);
    }

    private void Reset ()
    {
        m_CurrentIndex = 0;
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
                NextPlayer ();
            }
            // TODO : Set up button
        }
    }
}

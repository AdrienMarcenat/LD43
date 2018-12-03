using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class OnBattleGameEvent : GameEvent
{
    public OnBattleGameEvent(bool enter, EdgeView edge = null) : base("Game")
    {
        m_Enter = enter;
        m_Edge = edge;
    }

    public bool m_Enter;
    public EdgeView m_Edge;
}

public class BattlePanel : MonoBehaviour
{
    [SerializeField] List<Image> m_PlayerThumbnails;
    [SerializeField] List<Image> m_EnemiesThumbnails;
    [SerializeField] Button m_AttackButton;
    [SerializeField] Button m_DefendButton;
    [SerializeField] Button m_CapacityButton;

    private List<Character> m_Team;
    private List<Character> m_Enemies;
    private Character m_CurrentPlayer;
    private int m_CurrentIndex;
    private bool m_IsEnemyTurn;

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
            StartCoroutine (Init (battleEvent.m_Edge));
        }
        else
        {
            StartCoroutine(Reset ());
        }
    }

    IEnumerator Init (EdgeView edge)
    {
        yield return null;
        m_CurrentIndex = 0;
        m_IsEnemyTurn = false;
        m_Team = new List<Character> ();
        int index = 0;
        foreach (CharacterModel model in TeamManagerProxy.Get ().GetTeam ().Values)
        {
            m_Team.Add (new Character(model));
            m_PlayerThumbnails[index].sprite = RessourceManager.LoadSprite ("Models/" + model.GetClass().ToString(), 0);
            index++;
        }
        m_CurrentPlayer = m_Team[0];
        ActivateButtons (true);

        m_Enemies = new List<Character> ();
        index = 0;
        foreach (ECharacterClass characterClass in edge.GetEdgeResource ().GetEnemies ())
        {
            m_Enemies.Add (new Character (new CharacterModel("Enemy", characterClass)));
            m_EnemiesThumbnails[index].sprite = RessourceManager.LoadSprite ("Models/" + characterClass.ToString (), 0);
            index++;
        }
        BattleManagerProxy.Get ().Init (m_Team, m_Enemies);
        UpdateUI ();
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
        if(m_IsEnemyTurn)
        {
            return;
        }
        m_CurrentIndex++;
        if (m_CurrentIndex == m_Team.Count)
        {
            BattleManagerProxy.Get ().ApplyActions ();
            BattleManagerProxy.Get ().TurnEnd ();
            m_CurrentIndex = 0;
            StartCoroutine (EnemyTurn ());
        }
        else
        {
            m_CurrentPlayer = m_Team[m_CurrentIndex];
            if(m_CurrentPlayer.IsBound ())
            {
                NextPlayer ();
                return;
            }
            if (m_CurrentPlayer.IsDead ())
            {
                NextPlayer ();
                return;
            }
        }
        UpdateUI ();
    }

    private void UpdateUI ()
    {
        int index = 0;
        foreach (Character c in m_Team)
        {
            if (c.IsBound ())
            {
                m_PlayerThumbnails[index].color = Color.blue;
            }
            else if (c.IsDead ())
            {
                m_PlayerThumbnails[index].color = Color.black;
            }
            else
            {
                m_PlayerThumbnails[index].color = Color.white;
            }
            index++;
        }
        index = 0;
        foreach (Character c in m_Enemies)
        {
            if (c.IsBound ())
            {
                m_EnemiesThumbnails[index].color = Color.blue;
            }
            if (c.IsDead ())
            {
                m_EnemiesThumbnails[index].color = Color.black;
            }
            else
            {
                m_EnemiesThumbnails[index].color = Color.white;
            }
            index++;
        }
        m_PlayerThumbnails[m_CurrentIndex].color = Color.green;
        m_CapacityButton.GetComponentInChildren<Text> ().text = m_CurrentPlayer.GetModel ().GetCapacity ().ToString ();
    }


    IEnumerator EnemyTurn ()
    {
        m_IsEnemyTurn = true;
        ActivateButtons (false);
        foreach (Character enemy in m_Enemies)
        {
            if(enemy.IsBound() || enemy.IsDead())
            {
                continue;
            }
            m_CurrentPlayer = enemy;
            int randomChoice = Random.Range (0, 1);
            switch(randomChoice)
            {
                case 0:
                    Attack ();
                    break;
                case 1:
                    Defend ();
                    break;
                case 2:
                    UseCapacity ();
                    break;
            }
        }
        yield return new WaitForSecondsRealtime (0.5f);
        BattleManagerProxy.Get ().ApplyActions ();
        BattleManagerProxy.Get ().TurnEnd ();
        ActivateButtons (true);
        m_IsEnemyTurn = false;
        UpdateUI ();
    }

    private void ActivateButtons(bool activate)
    {
        m_AttackButton.interactable = activate;
        m_CapacityButton.interactable = activate;
        m_DefendButton.interactable = activate;
    }
}

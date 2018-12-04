using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Assertions;

public class OnBattleGameEvent : GameEvent
{
    public OnBattleGameEvent (bool enter) : base ("Game")
    {
        m_Enter = enter;
    }

    public bool m_Enter;
}

public class OnEdgeBattleGameEvent : GameEvent
{
    public OnEdgeBattleGameEvent(bool enter, EdgeView edge = null) : base("Game")
    {
        m_Enter = enter;
        m_Edge = edge;
    }

    public bool m_Enter;
    public EdgeView m_Edge;
}

public class OnNodeBattleGameEvent : GameEvent
{
    public OnNodeBattleGameEvent(bool enter, NodeView node = null) : base ("Game")
    {
        m_Enter = enter;
        m_Node = node;
    }

    public bool m_Enter;
    public NodeView m_Node;
}

public class BattlePanel : MonoBehaviour
{
    [SerializeField] List<Image> m_PlayerThumbnails;
    [SerializeField] List<Image> m_EnemiesThumbnails;
    [SerializeField] Button m_AttackButton;
    [SerializeField] Button m_DefendButton;
    [SerializeField] Button m_CapacityButton;
    [SerializeField] Text m_ActivePlayerName;
    [SerializeField] float m_AnimationTime = 2f;

    private List<Character> m_Team;
    private List<Character> m_Enemies;
    private Dictionary<Character, Animator> m_Animators;
    private Dictionary<Character, Image> m_Healths;
    private Character m_CurrentPlayer;
    private int m_CurrentIndex;
    private bool m_IsEnemyTurn;
    private bool m_ShoudDoEnemyTurn = false;
    private bool m_IsApplyingAction = false;
    private bool m_IsInBattle = false;

    private void Awake ()
    {
        this.RegisterAsListener ("Game", typeof (OnBattleGameEvent), typeof (OnEdgeBattleGameEvent), typeof (OnNodeBattleGameEvent));
        gameObject.SetActive (false);
    }

    public void OnGameEvent (OnBattleGameEvent battleEvent)
    {
        if (battleEvent.m_Enter)
        {
            // This should not happen
        }
        else
        {
            StartCoroutine (Reset ());
        }
    }

    public void OnGameEvent (OnEdgeBattleGameEvent battleEvent)
    {
        if (battleEvent.m_Enter)
        {
            gameObject.SetActive (true);
            StartCoroutine (Init (battleEvent.m_Edge.GetEdgeResource().GetEnemies()));
        }
        else
        {
            StartCoroutine(Reset ());
        }
    }

    public void OnGameEvent (OnNodeBattleGameEvent battleEvent)
    {
        if (battleEvent.m_Enter)
        {
            gameObject.SetActive (true);
            StartCoroutine (Init (battleEvent.m_Node.GetNodeResource().GetEnemies()));
        }
        else
        {
            StartCoroutine (Reset ());
        }
    }

    IEnumerator Init (List<ECharacterClass> enemies)
    {
        yield return null;
        m_CurrentIndex = 0;
        m_ShoudDoEnemyTurn = false;
        m_IsEnemyTurn = false;
        m_IsApplyingAction = false;
        m_Team = new List<Character> ();
        m_Healths = new Dictionary<Character, Image> ();
        m_Animators = new Dictionary<Character, Animator> ();
        int index = 0;
        foreach (CharacterModel model in TeamManagerProxy.Get ().GetSortedTeam ())
        {
            m_Team.Add (new Character(model));
            m_PlayerThumbnails[index].gameObject.SetActive (true);
            m_PlayerThumbnails[index].sprite = RessourceManager.LoadSprite ("Models/" + model.GetClass().ToString(), 0);
            m_Animators.Add (m_Team[index], m_PlayerThumbnails[index].GetComponent<Animator>());
            m_Healths.Add (m_Team[index], m_PlayerThumbnails[index].transform.Find("Health").GetComponent<Image> ());
            index++;
        }
        for (int i = index; i < m_PlayerThumbnails.Count; ++i)
        {
            m_PlayerThumbnails[i].gameObject.SetActive (false);
        }
        m_CurrentPlayer = m_Team[0];
        ActivateButtons (true);

        m_Enemies = new List<Character> ();
        index = 0;
        foreach (ECharacterClass characterClass in enemies)
        {
            m_Enemies.Add (new Character (new CharacterModel("Enemy", characterClass)));
            m_EnemiesThumbnails[index].gameObject.SetActive (true);
            m_EnemiesThumbnails[index].sprite = RessourceManager.LoadSprite ("Models/" + characterClass.ToString (), 0);
            m_Animators.Add (m_Enemies[index], m_EnemiesThumbnails[index].GetComponent<Animator> ());
            m_Healths.Add (m_Enemies[index], m_EnemiesThumbnails[index].transform.Find ("Health").GetComponent<Image> ());
            index++;
        }
        for (int i = index; i < m_EnemiesThumbnails.Count; ++i)
        {
            m_EnemiesThumbnails[i].gameObject.SetActive (false);
        }
        BattleManagerProxy.Get ().Init (m_Team, m_Enemies);
        m_IsInBattle = true;
        UpdateUI ();
    }
    

    IEnumerator Reset ()
    {
        yield return null;
        m_CurrentIndex = 0;
        BattleManagerProxy.Get ().Shutdown ();
        new GameFlowEvent (EGameFlowAction.SuccessEdge).Push ();
        gameObject.SetActive (false);
        m_IsInBattle = false;
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
        EAction action = EAction.Defense;
        switch(m_CurrentPlayer.GetModel().GetCapacity())
        {
            case ECharacterCapacity.Heal:
                action = EAction.Heal;
                break;
            case ECharacterCapacity.Bound:
                action = EAction.Bound;
                break;
        }
        BattleManagerProxy.Get ().AddAction (action, m_CurrentPlayer);
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
            m_CurrentIndex = 0;
            ActivateButtons (false);
            StartCoroutine (ApplyActions ());
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
    }

    IEnumerator ApplyActions()
    {
        m_IsApplyingAction = true;
        BattleAction action  = BattleManagerProxy.Get ().DequeueAction ();
        while(action != null)
        {
            if(action.m_Source.IsDead() || action.m_Target.IsDead())
            {
                action = BattleManagerProxy.Get ().DequeueAction ();
                continue;
            }
            Animator source = m_Animators[action.m_Source];
            Animator target = m_Animators[action.m_Target];
            switch (action.m_Type)
            {
                case EAction.Attack:
                    Assert.IsTrue (source != target);
                    source.SetTrigger ("Attack");
                    target.SetTrigger ("Hit");
                    break;
                case EAction.Defense:
                    source.SetTrigger ("Heal");
                    break;
                case EAction.Heal:
                case EAction.Protect:
                    if (source != target)
                    {
                        source.SetTrigger ("Defense");
                        target.SetTrigger ("Heal");
                    }
                    else
                    {
                        source.SetTrigger ("Heal");
                    }
                    break;
                case EAction.Bound:
                    Assert.IsTrue (source != target);
                    source.SetTrigger ("Attack");
                    target.SetTrigger ("Heal");
                    break;
            }
            // All animations are 1 second long
            yield return new WaitForSeconds (m_AnimationTime);
            action.ApplyAction ();
            action = BattleManagerProxy.Get ().DequeueAction ();
        }
        BattleManagerProxy.Get ().TurnEnd ();
        ChangeTurn ();
        m_IsApplyingAction = false;
    }

    private void UpdateUI ()
    {
        int index = 0;
        foreach (Character c in m_Team)
        {
            m_Animators[c].SetBool ("Bound", c.IsBound ());
            m_Animators[c].SetBool ("Dead", c.IsDead ());
            m_Animators[c].SetBool ("Active", false);
            float fraction = Mathf.Clamp01 (((float)c.GetCurrentHealth()) / c.GetModel().GetVitality ());
            Image health = m_Healths[c];
            health.fillAmount  = fraction;
            index++;
        }
        index = 0;
        foreach (Character c in m_Enemies)
        {
            m_Animators[c].SetBool ("Bound", c.IsBound ());
            m_Animators[c].SetBool ("Dead", c.IsDead ());
            m_Animators[c].SetBool ("Active", false);
            float fraction = Mathf.Clamp01 (((float)c.GetCurrentHealth ()) / c.GetModel ().GetVitality ());
            Image health = m_Healths[c];
            health.fillAmount = fraction;
            index++;
        }
        if (!m_IsEnemyTurn)
        {
            m_Animators[m_CurrentPlayer].SetBool ("Active", !m_IsApplyingAction);
            ECharacterCapacity capactity = m_CurrentPlayer.GetModel ().GetCapacity ();
            m_CapacityButton.interactable = capactity != ECharacterCapacity.None;
            m_CapacityButton.GetComponentInChildren<Text> ().text = capactity.ToString ();
            m_ActivePlayerName.text = m_CurrentPlayer.GetModel ().GetName ();
        }
    }

    void ChangeTurn ()
    {
        m_IsEnemyTurn = !m_IsEnemyTurn;
        ActivateButtons (!m_IsEnemyTurn);
        m_ShoudDoEnemyTurn = m_IsEnemyTurn;
        if (!m_IsEnemyTurn)
        {
            m_CurrentPlayer = m_Team[0];
        }
    }

    private void Update ()
    {
        if(m_IsInBattle)
        {
            UpdateUI ();
        }
        if (m_ShoudDoEnemyTurn)
        {
            EnemyTurn ();
        }
    }

    private void EnemyTurn()
    {
        m_ShoudDoEnemyTurn = false;
        foreach (Character enemy in m_Enemies)
        {
            if (enemy.IsBound () || enemy.IsDead ())
            {
                continue;
            }
            m_CurrentPlayer = enemy;
            int randomChoice = Random.Range (0, 1);
            switch (randomChoice)
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
        StartCoroutine (ApplyActions ());
        return;
    }

    private void ActivateButtons(bool activate)
    {
        m_AttackButton.interactable = activate;
        m_CapacityButton.interactable = activate;
        m_DefendButton.interactable = activate;
    }
}

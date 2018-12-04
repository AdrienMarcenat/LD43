using System.Collections.Generic;

public class BattleManager
{
    private bool m_IsPlayerTurn = true;
    private Stack<Character> m_PlayerCharacters;
    private Stack<Character> m_EnnemyCharacters;
    private Queue<BattleAction> m_Actions;

    public void Init(List<Character> team, List<Character> enemies)
    {
        m_IsPlayerTurn = true;
        m_Actions = new Queue<BattleAction> ();
        m_PlayerCharacters = new Stack<Character> ();
        m_EnnemyCharacters = new Stack<Character> ();
        foreach (Character c in team)
        {
            m_PlayerCharacters.Push(c);
        }
        foreach (Character c in enemies)
        {
            m_EnnemyCharacters.Push (c);
        }
    }

    public void Shutdown()
    {
        m_Actions.Clear ();
    }

    public void NextPlayerTurn ()
    {
        m_IsPlayerTurn = !m_IsPlayerTurn;
    }

    public void SetupTurn ()
    {
        m_Actions.Clear ();
    }

    public void AddAction(EAction action, Character chara)
    {
        BattleAction newAction = null;
        Character playerPeek = m_PlayerCharacters.Peek ();
        Character enemyPeek = m_EnnemyCharacters.Peek ();
        switch (action)
        {
            case EAction.Attack:
                newAction = new Attack (chara.GetStrength (), chara, m_IsPlayerTurn ? enemyPeek : playerPeek );
                break;
            case EAction.Defense:
                newAction = new Defense (chara.GetMagic (), chara, chara);
                break;
            case EAction.Heal:
                newAction = new Heal (chara.GetMagic (), chara, m_IsPlayerTurn ? playerPeek : enemyPeek);
                break;
            case EAction.Protect:
                newAction = new Protect (chara, m_IsPlayerTurn ? playerPeek : enemyPeek);
                break;
            case EAction.Bound:
                newAction = new Bound (chara, m_IsPlayerTurn ? playerPeek : enemyPeek);
                break;
        }
        
        m_Actions.Enqueue (newAction);
    }

    public BattleAction DequeueAction ()
    {
        if(m_Actions.Count != 0)
        {
            return m_Actions.Dequeue ();
        }
        return null;
    }

    public void TurnEnd ()
    {
        if (m_PlayerCharacters.Count == 0)
        {
            new GameFlowEvent (EGameFlowAction.GameOver).Push ();
            new OnBattleGameEvent (false).Push ();
            return;
        }

        if (m_EnnemyCharacters.Count == 0)
        {
            new OnBattleGameEvent (false).Push ();
            return;
        }

        NextPlayerTurn ();
    }

    public void Attack (int damage)
    {
        if (m_IsPlayerTurn)
        {
            if(m_EnnemyCharacters.Peek ().TakeDamage (damage))
            {
                m_EnnemyCharacters.Pop ();
                if(m_EnnemyCharacters.Count == 0)
                {
                    m_Actions.Clear ();
                }
            }
        }
        else
        {
            if(m_PlayerCharacters.Peek ().TakeDamage (damage))
            {
                Character player = m_PlayerCharacters.Pop ();
                TeamManagerProxy.Get ().RemoveCharacter (player.GetModel ().GetName ());
                if (m_PlayerCharacters.Count == 0)
                {
                    m_Actions.Clear ();
                }
            }
        }
    }

    public void Defense (int resistance)
    {
        if (m_IsPlayerTurn)
        {
            m_PlayerCharacters.Peek ().Resistance (resistance);
            // Visuals?
        }
        else
        {
            m_EnnemyCharacters.Peek ().Resistance (resistance);
            // Visuals?
        }
    }

    public void ResetCondition ()
    {
        if (m_IsPlayerTurn)
        {
            m_EnnemyCharacters.Peek ().ResetCondition ();
        }
        else
        {
            m_PlayerCharacters.Peek ().ResetCondition ();
        }
    }

    public void Heal (int heal)
    {
        if (m_IsPlayerTurn)
        {
            m_PlayerCharacters.Peek ().Heal (heal);
            // Visuals?
        }
        else
        {
            m_EnnemyCharacters.Peek ().Heal (heal);
            // Visuals?
        }
    }

    public void Bound ()
    {
        if (m_IsPlayerTurn)
        {
            m_EnnemyCharacters.Peek ().Bound ();
        }
        else
        {
            m_PlayerCharacters.Peek ().Bound ();
        }
    }

    public void Protect ()
    {
        if (m_IsPlayerTurn)
        {
            m_PlayerCharacters.Peek ().Protected ();
        }
        else
        {
            m_EnnemyCharacters.Peek ().Protected ();
        }
    }
}

public class BattleManagerProxy : UniqueProxy<BattleManager>
{

}

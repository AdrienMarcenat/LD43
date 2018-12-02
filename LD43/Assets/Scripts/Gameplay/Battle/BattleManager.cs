using System.Collections.Generic;

public class BattleManager
{
    private bool m_IsPlayerTurn = true;
    private Stack<Character> m_PlayerCharacters;
    private Stack<Character> m_EnnemyCharacters;
    private Queue<BattleAction> m_Actions;

    public void NextPlayerTurn ()
    {
        m_IsPlayerTurn = !m_IsPlayerTurn;
    }

    public void SetupTurn ()
    {
        // TODO: Setup the turn depending on who's turn it is (player or ennemy)
    }

    public void AddAction(BattleAction action)
    {
        m_Actions.Enqueue (action);
    }

    public void ApplyActions ()
    {
        foreach (BattleAction action in m_Actions)
        {
            m_Actions.Dequeue ().ApplyAction ();
        }
        new BattleEvent (EBattleAction.Effect).Push ();
    }

    public void TurnEnd ()
    {
        if (m_PlayerCharacters.Count == 0)
        {
            new BattleEvent (EBattleAction.Failure).Push ();
            return;
        }

        if (m_EnnemyCharacters.Count == 0)
        {
            new BattleEvent (EBattleAction.Success).Push ();
            return;
        }

        new BattleEvent (EBattleAction.Nothing).Push ();
    }

    public void Attack (int damage)
    {
        if (m_IsPlayerTurn)
        {
            m_EnnemyCharacters.Peek ().TakeDamage (damage);
            // Implement Character death
        }
        else
        {
            m_PlayerCharacters.Peek ().TakeDamage (damage);
            // Implement character death
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
}

public class BattleManagerProxy : UniqueProxy<BattleManager>
{

}

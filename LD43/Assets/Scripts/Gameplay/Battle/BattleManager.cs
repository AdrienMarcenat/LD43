using System.Collections.Generic;

public class BattleManager
{
    private bool m_IsPlayerTurn = true;
    private List<Character> m_PlayerCharacters;
    private List<Character> m_EnnemyCharacters;
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
}

public class BattleManagerProxy : UniqueProxy<BattleManager>
{

}

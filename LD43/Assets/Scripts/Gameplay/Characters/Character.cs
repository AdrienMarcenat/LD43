using System.Collections.Generic;

public enum EAction
{
    Attack,
    Defense,
    Heal,
    Protect,
    Bound
}

public class Character
{
    protected CharacterModel m_Model;
    protected List<EAction> m_BattleActions;
    protected int m_CurrentHealth;
    protected int m_Resistance = 0;
    protected bool m_Protected = false;
    protected bool m_Bound = false;
    
    public Character(CharacterModel model)
    {
        m_Model = model;
        m_CurrentHealth = model.GetVitality ();
    }

    public List<EAction> GetBattleActions ()
    {
        return m_BattleActions;
    }

    public CharacterModel GetModel()
    {
        return m_Model;
    }

    public bool TakeDamage (int damage)
    {
        // If character is protected
        if (m_Protected)
        {
            return false;
        }
        m_CurrentHealth -= System.Math.Max(damage - m_Resistance, 0);
        return IsDead ();
    }

    public void Heal (int heal)
    {
        m_CurrentHealth = System.Math.Max(m_CurrentHealth + heal, m_Model.GetVitality ());
    }

    public bool IsDead()
    {
        return m_CurrentHealth <= 0;
    }

    public void Resistance (int resistance)
    {
        m_Resistance = resistance;
    }

    public void Protected ()
    {
        m_Protected = true;
    }

    public void Bound ()
    {
        m_Bound = true;
    }

    public bool IsBound ()
    {
        return m_Bound;
    }

    public void ResetCondition ()
    {
        m_Resistance = 0;
        m_Protected = false;
        m_Bound = false;
    }

    public int GetStrength ()
    {
        return m_Model.GetStrength ();
    }

    public int GetMagic ()
    {
        return m_Model.GetMagic ();
    }
}

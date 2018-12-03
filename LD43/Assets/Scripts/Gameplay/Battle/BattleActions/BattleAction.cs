public abstract class BattleAction
{
    public abstract void ApplyAction ();

    public BattleAction(Character source, Character target, EAction type)
    {
        m_Source = source;
        m_Target = target;
        m_Type = type;
    }

    public Character m_Source;
    public Character m_Target;
    public EAction m_Type;
}

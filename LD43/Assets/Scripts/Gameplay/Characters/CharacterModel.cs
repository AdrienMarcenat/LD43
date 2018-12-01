
public class CharacterModel
{
    private string m_Name;
    private int m_Class;
    private int m_Id;
    private int m_Speed;
    private int m_Strength;
    
    public void SetClass (int newClass)
    {
        m_Class = newClass;
    }

    public void SetId (int newId)
    {
        m_Id = newId;
    }

    public void SetName (string newName)
    {
        m_Name = newName;
    }

    public void SetSpeed (int newSpeed)
    {
        m_Speed = newSpeed;
    }

    public void SetStrength (int newStrength)
    {
        m_Strength = newStrength;
    }

    public int GetClass ()
    {
        return m_Class;
    }

    public int GetId ()
    {
        return m_Id;
    }

    public string GetName ()
    {
        return m_Name;
    }

    public int GetSpeed ()
    {
        return m_Speed;
    }

    public int GetStrength ()
    {
        return m_Strength;
    }
}

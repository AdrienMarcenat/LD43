public enum ECharacterCapacity
{
    Heal,
    Purify
}

public enum ECharacterClass
{
    Soldier,
    Priest,
    FireMage
}

public class CharacterModel
{
    private string m_Name;
    private ECharacterClass m_Class;
    private int m_Id;
    private int m_Speed;
    private int m_Strength;
    private ECharacterCapacity m_Capacity;
    
    public void SetClass (ECharacterClass newClass)
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

    public ECharacterClass GetClass ()
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

    public ECharacterCapacity UseCapacity ()
    {
        return m_Capacity;
    }
}

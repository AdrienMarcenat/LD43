using System.IO;
using UnityEngine;

public enum ECharacterCapacity
{
    Heal,
    Purify,
    None
}

public enum ECharacterClass
{
    Prince,
    Soldier,
    Priest,
    FireMage,
    Monster,
    None
}

public class CharacterModel
{
    private string m_Name;
    private ECharacterClass m_Class;
    private int m_Id;
    private int m_Speed;
    private int m_Strength;
    private int m_Vitality;
    private int m_Magic;
    private ECharacterCapacity m_Capacity;
    
    public CharacterModel(string name, ECharacterClass newClass)
    {
        m_Name = name;
        m_Class = newClass;
        Build ();
    }

    private void Build ()
    {
        string filename = "/CharacterModels/" + m_Class + ".txt";
        filename = Application.streamingAssetsPath + filename;

        string[] lines = File.ReadAllLines (filename);
        SetId (int.Parse(lines[0]));
        SetSpeed (int.Parse (lines[1]));
        SetStrength (int.Parse (lines[2]));
        SetVitality (int.Parse (lines[3]));
        SetMagic (int.Parse (lines[4]));
        m_Capacity = (ECharacterCapacity)System.Enum.Parse (typeof(ECharacterCapacity), lines[5]);
    }

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

    public void SetVitality (int newVitality)
    {
        m_Vitality = newVitality;
    }

    public void SetMagic (int newMagic)
    {
        m_Magic = newMagic;
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

    public int GetVitality ()
    {
        return m_Vitality;
    }

    public int GetMagic ()
    {
        return m_Magic;
    }

    public ECharacterCapacity GetCapacity ()
    {
        return m_Capacity;
    }
}

using System.Collections.Generic;

public interface ITeamManagerInterface
{
    void AddCharacter (CharacterModel newCharacter);
    void RemoveCharacter (string characterId);
    CharacterModel GetCharacter (string characterId);
    void UseCharacterCapacity (string characterId);
    bool IsEnoughCharacters (int minCharacters);
    bool IsNotTooMuchCharacters (int maxCharacters);
    bool IsInRangeCharacters (int minCharacters, int maxCharacters);
    bool IsCharacterClass (ECharacterClass characterClass);
    Dictionary<string, CharacterModel> GetTeam();
}

public class TeamManager : ITeamManagerInterface
{
    private Dictionary<string, CharacterModel> m_Characters;

    public TeamManager()
    {
        m_Characters = new Dictionary<string, CharacterModel> ();
        AddCharacter (new CharacterModel ("Prince", ECharacterClass.Prince));
    }

    public Dictionary<string, CharacterModel> GetTeam ()
    {
        return m_Characters;
    }

    public void AddCharacter (CharacterModel newCharacter)
    {
        m_Characters.Add (newCharacter.GetName (), newCharacter);
    }

    public void RemoveCharacter(string characterId)
    {
        // TODO: Activate character dialog
        m_Characters.Remove (characterId);
    }

    public CharacterModel GetCharacter (string characterId)
    {
        CharacterModel temp;
        if (m_Characters.TryGetValue (characterId, out temp))
        {
            return temp;
        }

        return null;
    }

    public void UseCharacterCapacity (string characterId)
    {
        ECharacterCapacity capacity = m_Characters[characterId].GetCapacity ();

        switch (capacity)
        {
            case ECharacterCapacity.Heal:
                // TODO: Do the Heal
                break;
            case ECharacterCapacity.Purify:
                // TODO: PURIFY ALL INFIDELS
                break;
        }
    }

    public bool IsEnoughCharacters (int minCharacters)
    {
        return (m_Characters.Count >= minCharacters);
    }

    public bool IsNotTooMuchCharacters (int maxCharacters)
    {
        return (m_Characters.Count <= maxCharacters);
    }

    public bool IsInRangeCharacters (int minCharacters, int maxCharacters)
    {
        return ((m_Characters.Count >= minCharacters) && (m_Characters.Count <= maxCharacters));
    }

    public bool IsCharacterClass (ECharacterClass characterClass)
    {
        foreach (CharacterModel character in m_Characters.Values)
        {
            if (character.GetClass () == characterClass)
            {
                return true;
            }
        }

        return false;
    }
}

public class TeamManagerProxy : UniqueProxy<ITeamManagerInterface>
{

}
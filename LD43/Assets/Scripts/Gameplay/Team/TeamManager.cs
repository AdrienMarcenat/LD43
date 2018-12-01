using System.Collections.Generic;

public interface ITeamManagerInterface
{
    void AddCharacter (CharacterModel newCharacter);
    void RemoveCharacter (int characterId);
    CharacterModel GetCharacter (int characterId);
    bool IsEnoughCharacters (int minCharacters);
    bool IsNotTooMuchCharacters (int maxCharacters);
    bool IsInRangeCharacters (int minCharacters, int maxCharacters);
    bool IsCharacterClass (int characterClass);
}

public class TeamManager : ITeamManagerInterface
{

    private Dictionary<int, CharacterModel> m_Characters;

    public TeamManager()
    {
        m_Characters = new Dictionary<int, CharacterModel> ();
    }

    public void AddCharacter (CharacterModel newCharacter)
    {
        m_Characters.Add (newCharacter.GetId (), newCharacter);
    }

    public void RemoveCharacter(int characterId)
    {
        m_Characters.Remove (characterId);
    }

    public CharacterModel GetCharacter (int characterId)
    {
        CharacterModel temp;
        if (m_Characters.TryGetValue (characterId, out temp))
        {
            return temp;
        }

        return null;
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

    public bool IsCharacterClass (int characterClass)
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
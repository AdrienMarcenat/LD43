using System.Collections.Generic;

public interface TeamManagerInterface
{
    void AddCharacter (CharacterModel newCharacter);
    void RemoveCharacter (int characterId);
    CharacterModel GetCharacter (int characterId);
}

public class TeamManager : TeamManagerInterface
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
}

public class TeamManagerProxy : UniqueProxy<TeamManagerInterface>
{

}
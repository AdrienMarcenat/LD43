using System.Collections.Generic;

public interface ITeamManagerInterface
{
    void AddCharacter (CharacterModel newCharacter);
    void RemoveCharacter (string characterId);
    void RemoveCharacterWithDialogue (string characterId);
    CharacterModel GetCharacter (string characterId);
    void UseCharacterCapacity (string characterId);
    bool IsEnoughCharacters (int minCharacters);
    bool IsNotTooMuchCharacters (int maxCharacters);
    bool IsInRangeCharacters (int minCharacters, int maxCharacters);
    bool IsCharacterClass (ECharacterClass characterClass);
    void WaitForDiversion ();
    void GetKey ();
    bool HasKey ();
    Dictionary<string, CharacterModel> GetTeam();
    List<CharacterModel> GetSortedTeam ();
    void SayGoodbye ();
}

public class TeamManager : ITeamManagerInterface
{
    private Dictionary<string, CharacterModel> m_Characters;
    private bool m_WaitingForDiversion = false;
    private bool m_HasKey = false;

    public TeamManager()
    {
        m_Characters = new Dictionary<string, CharacterModel> ();
    }

    public Dictionary<string, CharacterModel> GetTeam ()
    {
        return m_Characters;
    }

    public void AddCharacter (CharacterModel newCharacter)
    {
        if(m_Characters.Count == 6)
        {
            return;
        }
        m_Characters.Add (newCharacter.GetName (), newCharacter);
        new UpdateUIGameEvent ().Push ();
    }

    public void RemoveCharacter (string characterId)
    {
        m_Characters.Remove (characterId);
        new UpdateUIGameEvent ().Push ();
    }

    public void RemoveCharacterWithDialogue (string characterId)
    {
        if (!characterId.Equals ("Prince"))
        {
            RemoveCharacter (characterId);

            if (m_WaitingForDiversion)
            {
                new OnCharacterDiversionEvent ().Push ();
                m_WaitingForDiversion = false;
            }
        }
        if (!m_WaitingForDiversion)
        {
            DialogueManagerProxy.Get ().TriggerDialogue (characterId + " LetGo");
        }
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
    
    public List<CharacterModel> GetSortedTeam()
    {
        List<CharacterModel> sortedList = new List<CharacterModel> ();
        foreach(CharacterModel c in m_Characters.Values)
        {
            sortedList.Add (c);
        }
        sortedList.Sort (SortByID);
        return sortedList;
    }

    static int SortByID (CharacterModel p1, CharacterModel p2)
    {
        return p2.GetId ().CompareTo (p1.GetId ());
    }

    public void WaitForDiversion ()
    {
        m_WaitingForDiversion = true;
    }

    public void GetKey ()
    {
        m_HasKey = true;
    }

    public bool HasKey ()
    {
        return m_HasKey;
    }

    public void SayGoodbye ()
    {
        List<string> characterID = new List<string> ();
        foreach (CharacterModel model in m_Characters.Values)
        {
            if (!model.GetName ().Equals ("Prince"))
            {
                characterID.Add (model.GetName ());
            }
        }
        foreach (string name in characterID)
        {
            m_Characters.Remove (name);
        }
    }
}

public class TeamManagerProxy : UniqueProxy<ITeamManagerInterface>
{

}
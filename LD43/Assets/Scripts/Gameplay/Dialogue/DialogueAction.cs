using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IDialogueAction
{
    void Action ();
}

public class AddToTeamAction : IDialogueAction
{
    public string m_CharacterName;
    public ECharacterClass m_CharacterClass;

    public AddToTeamAction (string characterName, ECharacterClass characterClass)
    {
        m_CharacterName = characterName;
        m_CharacterClass = characterClass;
    }

    public void Action()
    {
        TeamManagerProxy.Get ().AddCharacter (new CharacterModel (m_CharacterName, m_CharacterClass));
    }
}

public class SayGoodbyeAction : IDialogueAction
{
    public SayGoodbyeAction ()
    {

    }

    public void Action ()
    {
        TeamManagerProxy.Get ().SayGoodbye ();
    }
}

public class RemoveFromTeamAction : IDialogueAction
{
    public string m_CharacterName;

    public RemoveFromTeamAction (string characterName)
    {
        m_CharacterName = characterName;
    }

    public void Action ()
    {
        TeamManagerProxy.Get ().RemoveCharacter (m_CharacterName);
    }
}

public class UseCapacityAction : IDialogueAction
{
    public string m_CharacterName;
    public ECharacterCapacity m_Capacity;

    public UseCapacityAction (string characterName, ECharacterCapacity capacity)
    {
        m_CharacterName = characterName;
        m_Capacity = capacity;
    }

    public void Action ()
    {
        TeamManagerProxy.Get ().UseCharacterCapacity (m_CharacterName);
    }
}

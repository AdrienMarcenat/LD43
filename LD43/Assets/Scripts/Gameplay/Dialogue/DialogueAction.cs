using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum EDialogueActionType
{
    AddToTeam,
    RemoveFromTeam,
    UseCapacity
}

public class DialogueActionGameEvent : GameEvent
{
    public DialogueActionGameEvent (string tag, DialogueAction action)
        : base (tag)
    {
        m_Action = action;
    }

    public DialogueAction GetAction ()
    {
        return m_Action;
    }

    private DialogueAction m_Action;
}

public class DialogueAction
{
    public EDialogueActionType m_ActionType;

    public DialogueAction(EDialogueActionType actionType)
    {
        m_ActionType = actionType;
    }
}

public class AddToTeamAction : DialogueAction
{
    public string m_CharacterName;
    public ECharacterClass m_CharacterClass;

    public AddToTeamAction (string characterName, ECharacterClass characterClass)
        : base(EDialogueActionType.AddToTeam)
    {
        m_CharacterName = characterName;
        m_CharacterClass = characterClass;
    }
}

public class RemoveFromTeamAction : DialogueAction
{
    public string m_CharacterName;

    public RemoveFromTeamAction (string characterName)
        : base (EDialogueActionType.RemoveFromTeam)
    {
        m_CharacterName = characterName;
    }
}

public class UseCapacityAction : DialogueAction
{
    public string m_CharacterName;
    public ECharacterCapacity m_Capacity;

    public UseCapacityAction (string characterName, ECharacterCapacity capacity)
        : base (EDialogueActionType.UseCapacity)
    {
        m_CharacterName = characterName;
        m_Capacity = capacity;
    }
}

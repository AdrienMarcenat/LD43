using System.Collections.Generic;

public class CommandStack
{
    private Stack<Command> m_CommandStack;

    public CommandStack ()
    {
        m_CommandStack = new Stack<Command> ();
    }

    ~CommandStack()
    {
    }

    public void PushCommand(Command command)
    {
        m_CommandStack.Push (command);
    }

    public Command PopCommand ()
    {
        return m_CommandStack.Pop ();
    }

    public int GetNumberOfCommand()
    {
        return m_CommandStack.Count;
    }

    public void Undo()
    {
        if (GetNumberOfCommand () > 0)
        {
            PopCommand ().Undo ();
        }
    }

    public void Reset()
    {
        m_CommandStack.Clear ();
    }
}

public class CommandStackProxy : UniqueProxy<CommandStack>
{ }

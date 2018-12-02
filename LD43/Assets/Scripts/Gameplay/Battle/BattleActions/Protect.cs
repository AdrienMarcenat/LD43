public class Protect : BattleAction
{

    public Protect ()
    {

    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Protect ();
    }
}

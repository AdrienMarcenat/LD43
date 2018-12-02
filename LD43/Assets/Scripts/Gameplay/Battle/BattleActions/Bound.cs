public class Bound : BattleAction
{
    public Bound ()
    {

    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Bound ();
    }
}

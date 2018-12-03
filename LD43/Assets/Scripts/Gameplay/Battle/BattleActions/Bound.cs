public class Bound : BattleAction
{
    public Bound (Character source, Character target)
        : base (source, target, EAction.Bound)
    {

    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Bound ();
    }
}

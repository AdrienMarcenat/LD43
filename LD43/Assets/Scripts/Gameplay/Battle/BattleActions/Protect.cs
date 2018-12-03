public class Protect : BattleAction
{

    public Protect (Character source, Character target)
        : base (source, target, EAction.Protect)
    {

    }

    public override void ApplyAction ()
    {
        BattleManagerProxy.Get ().Protect ();
    }
}

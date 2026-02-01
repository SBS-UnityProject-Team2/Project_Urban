using System.Collections;

public class Embers : Attack
{ 
    public override CardName Name => CardName.Embers;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Damage(user, damage, Element.Ruin);
        user.Deck.Copy(this);
    }
}
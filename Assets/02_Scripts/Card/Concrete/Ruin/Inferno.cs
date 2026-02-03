using System.Collections;
using UnityEngine;

public class Inferno : Attack
{   
    [SerializeField] GameObject HandInferno;
    public override CardName Name => CardName.Inferno;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        int extinctCount = user.Deck.ExtinctCardCount;
        int totalDamage = extinctCount * damage;
        
        Transform handSpawnPoint = GameObject.Find("Panel_MachineArm").transform;
        GameObject handEffect = Instantiate(HandInferno, handSpawnPoint.position, handSpawnPoint.rotation);

        yield return PlayEffect(target);
        target.Damage(user, totalDamage, Element.Ruin);
        Destroy(handEffect);
    }
}
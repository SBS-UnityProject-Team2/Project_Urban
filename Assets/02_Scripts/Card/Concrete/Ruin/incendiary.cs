    using UnityEngine;

    public class Incendiary : BuffCard
    {

    [SerializeField] private int addedDamage;
    
    public override CardName Name => CardName.Incendiary;

        public override int Use(Player player, Target target)
        {
            target.LoadedIncendiary(addedDamage);

            return cost;
        }
    }
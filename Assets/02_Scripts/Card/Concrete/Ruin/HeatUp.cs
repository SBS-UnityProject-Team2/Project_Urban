    using UnityEngine;

    public class HeatUp : BuffCard
    {
    [SerializeField] private int count;
    public override CardName Name => CardName.HeatUp;

        public override int Use(Player player, Target target)
        {
            target.Reinforce(count);

            return cost;
        }
    }
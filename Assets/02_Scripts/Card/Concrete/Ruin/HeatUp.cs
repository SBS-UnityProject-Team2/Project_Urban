    using UnityEngine;

    public class HeatUp : BuffCard
    {
    [SerializeField] private int count;
    public override CardName Name => CardName.HeatUp;

        public override int Use(Target target)
        {
            target.Reinforce(count);

            return cost;
        }
    }
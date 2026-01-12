    using UnityEngine;

    public class incendiary : BuffCard
    {

    [SerializeField] private int addedDamage;
    
    public override CardName Name => CardName.Incendiary;

        public override int Use(Target target)
        {
            target.Incendiary(addedDamage);

            return cost;
        }
    }
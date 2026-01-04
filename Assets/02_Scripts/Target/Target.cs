using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

abstract public class Target : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected Element element;

    [Header("Base View Settings")]
    [SerializeField] protected HealthView healthView;
    // 버프-디버프 관리
    protected StatusEffectList statusEffectList;
    
    // Status
    protected int attack;
    protected int block;
    protected int bleed;

    protected bool isStun;
 
    // Property
    public HealthController Health { get; protected set; }
    public bool IsStun => isStun;
    public int Bleed => bleed;

    public Element Element
    {
        get { return element; }
        set { element = value; }
    }

    // Event
    public UnityEvent<Target> OnDead { get; } = new();
    public UnityEvent<Target, bool> OnDamaged { get; } = new();

    public void Heal(int healPoint)
    {
        if (bleed > 0)
        {
            int bleedReduction = Mathf.Min(bleed, healPoint);
            bleed -= bleedReduction;
            healPoint -= bleedReduction;
        }
        
        if (healPoint > 0)
        {
            Health.IncreaseHp(healPoint);
        }
    }

    public void Damage(int hitPoint)
    {
        if (block != 0)
        {
            block--;

            return;
        }

        // 방패로 데미지 흡수
        int shieldPoints = Health.Protect;
        int damageToShield = Mathf.Min(shieldPoints, hitPoint);
        int remainingDamage = hitPoint - damageToShield;

        // 방패에 데미지 적용
        if (damageToShield > 0)
            Health.DecreaseProtect(damageToShield);

        // 남은 데미지를 체력에 적용
        if (remainingDamage > 0)
            Health.DecreaseHp(remainingDamage);

        if (Health.CurrentHp <= 0)
            OnDead?.Invoke(this);
        else
            OnDamaged?.Invoke(this, Health.Protect > 0);

        Debug.Log($"{gameObject.name} : {Health.CurrentHp}, hitPoint : {hitPoint}");
    }

    public void AddProtect(int protectPoint)
    {
        Health.IncreaseProtect(protectPoint);
    }

    public void AddBleed(int bleedCount)
    {
        bleed += bleedCount;
    }

    public void AddBlock(int blockCount = 1)
    {
        block += blockCount;
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect is TimedStatusEffect)
            statusEffectList.AddEffects(statusEffect as TimedStatusEffect);
        else
            statusEffect.Apply(this);
    }

    public void IncreaseAttack(int amount = 1)
    {
        attack += amount;
    }

    public void DecreaseAttack(int amount = 1)
    {
        attack -= amount;

        if (attack < 0)
            attack = 0;
    }
}

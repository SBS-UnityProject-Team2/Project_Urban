using UnityEngine;
using UnityEngine.Events;


abstract public class Target : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected Element element;

    [Header("Base View Settings")]
    [SerializeField] protected HealthView healthView;
    [SerializeField] protected StatusEffectListView statusEffectListView;
    // 버프-디버프 관리
    protected Status status;
    protected StatusEffectList statusEffectList;

    // Buff
    protected Reinforce reinforce;
    protected Armor armor;
    protected Dummy dummy;
    protected Refined refined;
    protected Incendiary incendiary;
    protected KineticVeil kineticVeil;
    protected SuperConduct superConduct;
    protected BioActiveShell bioActiveShell;
    protected Regeneration regeneration;
    protected Spike spike;


    protected Bleed bleed;
    
    // Status
    protected int block;
    protected int burn;
    protected int additionalDamage;
    protected int additionalDamageCount;
    protected float damageModifier;

    protected bool isStun;
 
    // Property
    public HealthController Health { get; protected set; }
    public Status Status => status;
    public bool IsStun => isStun;

    public Element Element
    {
        get { return element; }
        set { element = value; }
    }

    // Event
    public UnityEvent OnTurnStart = new();
    public UnityEvent OnTurnEnd = new();
    public UnityEvent<Target> OnAttack = new();
    public UnityEvent<Target, Target, bool> OnDamaged { get; } = new();
    public UnityEvent<Target> OnDead { get; } = new();

    protected virtual void Awake()
    {
        status = new Status();

        reinforce = new Reinforce();
        armor = new Armor();

        bleed = new Bleed(this);
        refined = new Refined(this);
        incendiary = new Incendiary(this);
        superConduct = new SuperConduct(this);
        regeneration = new Regeneration(this);
        

        statusEffectList = new(this);
        statusEffectListView.Bind(statusEffectList);

        OnTurnStart.AddListener(HandleTurnStart);
        OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Heal(int healPoint)
    {
       
        
        if (healPoint > 0)
        {
            Health.IncreaseHp(healPoint);
        }
    }

    public void Protect(int protectPoint)
    {
        Health.IncreaseProtect(protectPoint + status.Armor);
    }
    public void Damage(Target attacker, int hitPoint)
    {
        if (status.IsBlock) return;

        if (block != 0)
        {
            block--;

            return;
        }
        
        // 받는 데미지 증가
        hitPoint += (int)(hitPoint * damageModifier);

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
            OnDamaged?.Invoke(attacker, this, Health.Protect > 0);

        Debug.Log($"{gameObject.name} : {Health.CurrentHp}, hitPoint : {hitPoint}");
    }

    
    public void Reinforce(int count)
    {
        reinforce.Modify(status, count);
    }

    public void Armor(int count)
    {
        armor.Modify(status, count);
    }

    public void Dummy(int count)
    {
        dummy.Modify(status, count);
    }

    public void Refined(int turn)
    {
        refined.Apply(turn);
    }

    public void Incendiary(int count)
    {
        incendiary.Active(count);
    }

    public void KineticVeil(int turn)
    {
        kineticVeil.Apply(turn);
    }

    public void SuperConduct(int turn)
    {
        superConduct.Apply(turn);
    }

    public void BioActiveShell(int turn)
    {
        bioActiveShell.Apply(turn);
    }

    public void Regeneration(int turn)
    {
        regeneration.Apply(turn);
    }

    public void Bleed(int count)
    {
        bleed.Increase(count);
    }

    public void Spike(int count)
    {
        spike.Active(count);
    }



    public void AddBlock(int blockCount = 1)
    {
        block += blockCount;
    }

    public void IncreaseBurn(int burnCount = 1)
    {
        burn += burnCount;
    }

    public void DecreaseBurn(int burnCount = 1)
    {
        burn -= burnCount;

        if (burn < 0)
            burn = 0;
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect is TimedStatusEffect)
            statusEffectList.AddEffects(statusEffect as TimedStatusEffect);
        else
            statusEffect.Apply(this);
    }


    public void IncreaseDamageTaken(float amount = 1)
    {
        damageModifier += amount;
    }

    public void DecreaseDamageTaken(float amount = 1)
    {
        damageModifier -= amount;

        if (damageModifier < 0.0f)
            damageModifier = 0.0f;
    }

    public void IncreaseAdditionalDamage(int amount = 1)
    {
        additionalDamage += amount;
    }

    public void DecreaseAdditionalDamage(int amount = 1)
    {
        additionalDamage -= amount;

        if (additionalDamage < 0)
            additionalDamage = 0;
    }

    public void ResetAdditionalDamage()
    {
        additionalDamage = 0;
    }

    public void IncreaseAdditionalDamageCount(int amount = 1)
    {
        additionalDamageCount += amount;
    }

    public void DecreaseAdditionalDamageCount(int amount = 1)
    {
        additionalDamageCount -= amount;

        if (additionalDamageCount < 0)
            additionalDamageCount = 0;
    }

    
    private void HandleTurnStart()
    {
        Health.ResetProtect();
    }

    private void HandleTurnEnd()
    {
        statusEffectList.DecreaseTurn();
        Health.DecreaseHp(burn);
    }
}

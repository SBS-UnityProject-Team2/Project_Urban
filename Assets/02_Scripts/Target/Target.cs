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

    protected Weaken weaken;
    protected Broken broken;
    protected Bleed bleed;
    protected Burn burn;
    protected Poisoned poisoned;
    protected Stigma stigma;
    protected Frozen frozen;
    protected Anointed anointed;
    protected Delirium delirium;
    protected Infested infested;
    protected Scarred scarred;


 
    // Property
    public HealthController Health { get; protected set; }
    public Status Status => status;

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

        weaken = new Weaken(this);
        broken = new Broken(this);
        

        statusEffectList = new(this);
        statusEffectListView.Bind(statusEffectList);
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

        if (status.Dummy != 0)
        {

            return;
        }
        
        // 받는 데미지 증가
        if (status.IsBroken)
            hitPoint = broken.Modify(hitPoint);

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

    public void DebuffDamage(int hitPoint, Element attackType = Element.None)
    {
        if (status.IsBroken)
            hitPoint = broken.Modify(hitPoint);

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

    public void Spike(int count)
    {
        spike.Active(count);
    }

    public void Weaken(int turn)
    {
        weaken.Apply(turn);
    }

    public void Broken(int turn)
    {
        broken.Apply(turn);
    }

    public void Bleed(int count)
    {
        bleed.Increase(count);
    }



       public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect is TimedStatusEffect)
            statusEffectList.AddEffects(statusEffect as TimedStatusEffect);
        else
            statusEffect.Apply(this);
    }
}

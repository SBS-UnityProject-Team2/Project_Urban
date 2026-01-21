using System.Text;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Events;


abstract public class Target : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected Element element;
    [SerializeField] protected float damageModifier = 0.3f;

    [Header("Base View Settings")]
    [SerializeField] protected HealthView healthView;
    [SerializeField] protected StatusView statusView;
    // 버프-디버프 관리
    protected Status status;
 
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
    
    public UnityEvent<Target, Target> OnAttack = new();
    public UnityEvent OnHeal = new();
    public UnityEvent<Target, Target, bool> OnDamaged { get; } = new();
    public UnityEvent<Target> OnDead { get; } = new();

    protected virtual void Awake()
    {
        status = new Status(this);
        statusView?.Bind(status);
    }

    public void Heal(int healPoint)
    {
        int bleedStack = status.Bleed.Stack;
        status.Bleed.DecreaseStack(healPoint);

        healPoint -= bleedStack;

        if (healPoint > 0)
            Health.IncreaseHp(healPoint);
    }

    public void Protect(int protectPoint)
    {
        Health.IncreaseProtect(protectPoint + status.Armor.Stack);
    }

    private bool IsBlock()
    {
        if (status.Nullification.IsActive)
        {
            Debug.Log("Nullification Active");

            return true;
        }

        if (status.Blur.IsActive)
        {
            Debug.Log("Blur Active");

            status.Blur.DecreaseStack();
            return true;
        }

        return false;
    }

    private int CalcDamage(int hitPoint, Element attackType)
    {
        StringBuilder stringBuilder = new ();
        stringBuilder.Append($"Origin Damage : {hitPoint}, ");

        // 속성 상성 데미지 적용
        hitPoint = ModifyDamageByElement(hitPoint, attackType);
        stringBuilder.Append($"Element Modify Damage : {hitPoint}, ");
        
        // 버프 및 디버프
        int damage = hitPoint;
        // 파갑
        damage += status.Broken.Modify(hitPoint);
        stringBuilder.Append($"Broken Modify Damage : {damage}, ");

        // 속성별 추가데미지 디버프 
        damage += status.Anointed.Modify(hitPoint, attackType);
        stringBuilder.Append($"Anointed Modify Damage : {damage}, ");

        damage += status.Delirium.Modify(hitPoint, attackType);
        stringBuilder.Append($"Delirium Modify Damage : {damage}, ");

        damage += status.Infested.Modify(hitPoint, attackType);
        stringBuilder.Append($"Infested Modify Damage : {damage}");

        return damage;
    }

    private void DamageApplyProcess(int damage)
    {
        int shieldPoints = Health.Protect;
        int damageToShield = Mathf.Min(shieldPoints, damage);
        int remainingDamage = damage - damageToShield;

        // 방패에 데미지 적용
        if (damageToShield > 0)
            Health.DecreaseProtect(damageToShield);

        // 남은 데미지를 체력에 적용
        if (remainingDamage > 0)
            Health.DecreaseHp(remainingDamage);

        Debug.Log($"{gameObject.name} : {Health.CurrentHp}, hitPoint : {damage}");
    }

    private void InvokeDamagedEvent(Target attacker)
    {
        if (Health.CurrentHp <= 0)
            OnDead?.Invoke(this);
        else
            OnDamaged?.Invoke(attacker, this, Health.Protect > 0);
    }

    public void Damage(Target attacker, int hitPoint, Element attackType = Element.None)
    {
        if (IsBlock()) return;
        
        int damage = CalcDamage(hitPoint, attackType);
        damage += attacker.Status.Broken.Modify(damage);
        damage += attacker.Status.Attack;

        DamageApplyProcess(damage);
        InvokeDamagedEvent(attacker);
    }

    public void DebuffDamage(int hitPoint, Element attackType = Element.None)
    {
        if (IsBlock()) return;
        
        DamageApplyProcess(CalcDamage(hitPoint, attackType));

        if (Health.CurrentHp <= 0)
            OnDead?.Invoke(this);
    }

    private int ModifyDamageByElement(int hitPoint, Element attackType)
    {
        if (element == Element.None) return hitPoint;
        
        int modifiedDamage = 0;

        if (attackType == Element.Ruin)
        {
            if (element == Element.Bio) 
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (element == Element.Psychic) 
                modifiedDamage = (int)(-hitPoint * damageModifier);
        } 
        
        else if (attackType == Element.Psychic)
        {
            if (element == Element.Ruin) 
                modifiedDamage = (int)(hitPoint * damageModifier);
            
            if (element == Element.Bio) 
                modifiedDamage = (int)(-hitPoint * damageModifier);
        } 

        else if (attackType == Element.Bio)
        {
            if (element == Element.Psychic) 
                modifiedDamage = (int)(hitPoint * damageModifier);
            
            if (element == Element.Ruin) 
                modifiedDamage = (int)(-hitPoint * damageModifier);
        } 

        return hitPoint + modifiedDamage;
    }
    
    public void Reinforce(int count)
    {
        status.Reinforce.IncreaseStack(count);
    }

    public void Armor(int count)
    {
        status.Armor.IncreaseStack(count);
    }

    public void Blur(int count)
    {
        status.Blur.IncreaseStack(count);
    }

    public void Refined(int turn)
    {
        status.Refined.Apply(turn);
    }

    public void LoadedIncendiary(int count)
    {
        status.LoadedIncendiary.Active(count);
    }

    public void Searing(int count)
    {
        status.Searing.Active(count);
    }

    public void KineticVeil(int turn)
    {
        status.KineticVeil.Apply(turn);
    }

    public void ElectricVeil(int count)
    {
        status.ElectricVeil.Active(count);
    }

    public void Acceleration(int turn)
    {
        status.Acceleration.Apply(turn);
    }

    public void Nullification(int turn)
    {
        status.Nullification.Apply(turn);
    }

    public void BioActiveShell(int turn)
    {
        status.BioActiveShell.Apply(turn);
    }

    public void Regeneration(int turn)
    {
        status.Regeneration.Apply(turn);
    }

    public void ResourceTrade()
    {
        status.ResourceTrade.Active();
    }

    public void Spike(int count)
    {
        status.Spike.Active(count);
    }

    public void ElasticVeil(int count)
    {
        status.ElasticVeil.Active(count);
    }

    public void Weaken(int turn)
    {
        status.Weaken.Apply(turn);
    }

    public void Broken(int turn)
    {
        status.Broken.Apply(turn);
    }

    public void Exhaust(int turn)
    {
        status.Exhaust.Apply(turn);
    }

    public void Slow(int turn)
    {
        status.Slow.Apply(turn);
    }

    public void Bleed(int count)
    {
        status.Bleed.IncreaseStack(count);
    }

    public void Burn(int turn)
    {
        status.Burn.Apply(turn);
    }

    public void Poisoned(int turn)
    {
        status.Poisoned.Apply(turn);
    }

    public void Branded(int count)
    {
        status.Branded.Active(count);
    }

    public void Frozen(int turn)
    {
        status.Frozen.Apply(turn);
    }

    public void Anointed(int turn)
    {
        status.Anointed.Apply(turn);
    }

    public void Delirium(int turn)
    {
        status.Delirium.Apply(turn);
    }

    public void Infested(int turn)
    {
        status.Infested.Apply(turn);
    }

    public void Scarred(int count)
    {
        status.Scarred.Active(count);
    }

    public void Dizzy(int turn)
    {
        status.Dizzy.Apply(turn);
    }
}

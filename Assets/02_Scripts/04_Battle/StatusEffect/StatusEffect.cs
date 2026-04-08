using NUnit.Framework;
using UnityEngine.Events;

public abstract class StatusEffect
{
    protected Actor owner;

    private readonly StatusEffectDataEntry data;
    protected bool isActive = false;
    protected int duration;
    protected int stack;

    public bool IsActive => isActive;
    public int Duration => duration;
    public int Stack => stack;

    abstract public int StatusNumber { get; }    
    abstract public StatusEffectName Name { get; }

    public StatusEffectDataEntry Date => data;

    public StatusEffect(Actor owner)
    {
        this.owner = owner;
        data = StatusEffectManager.Instance.GetEffectData(Name);
    }
    
    // 상태 변경 이벤트
    public event UnityAction<StatusEffect> OnStatusChanged;

    public virtual void GiveStack(int stack = 1)
    {
        this.stack += stack;
        isActive = true;

        NotifyStatusChanged();
    }

    public virtual void GiveDuration(int duration = 1)
    {
        this.duration += duration;
        isActive = true;

        NotifyStatusChanged();
    }

    public virtual void RemoveStack(int stack = 1)
    {
        this.stack -= stack;

        if (this.stack <= 0)
        {
            RequestClear();

            return;
        }

        NotifyStatusChanged();
    }

    public virtual void RemoveDuration(int duration = 1)
    {
        this.duration -= duration;

        if (this.duration <= 0)
        {
            RequestClear();

            return;
        }

        NotifyStatusChanged();
    }

    public virtual void Clear()
    {
        stack = 0;
        duration = 0;   
        isActive = false;

        NotifyStatusChanged();
    }

    protected void NotifyStatusChanged()
    {
        OnStatusChanged?.Invoke(this);
    }    

    protected void RequestClear()
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.ClearBuffs,
            source = owner,
        };
        payload.Write(Name);
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);
    }
}
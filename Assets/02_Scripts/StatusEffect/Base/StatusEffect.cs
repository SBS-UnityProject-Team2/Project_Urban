using UnityEngine.Events;

public abstract class StatusEffect
{
    private bool isActive;
    public bool IsActive => isActive;

    abstract public int StatusNumber { get; }    
    abstract public StatusEffectName Name { get; }

    // 상태 변경 이벤트
    public event UnityAction<StatusEffect> OnStatusChanged;

    protected void NotifyStatusChanged()
    {
        OnStatusChanged?.Invoke(this);
    }

    protected void SetActive(bool value)
    {
        isActive = value;
        NotifyStatusChanged();
    }
}
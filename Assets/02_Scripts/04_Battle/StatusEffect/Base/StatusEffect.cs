using UnityEngine.Events;

public abstract class StatusEffect
{
    private bool isActive;
    private readonly StatusEffectDataEntry data;
    public bool IsActive => isActive;

    abstract public int StatusNumber { get; }    
    abstract public StatusEffectName Name { get; }

    public StatusEffectDataEntry Date => data;

    public StatusEffect()
    {
        // data = GameManager.Instance.GetEffectData(Name);
    }

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
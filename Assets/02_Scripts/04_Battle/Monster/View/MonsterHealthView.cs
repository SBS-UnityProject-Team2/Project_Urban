using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthView : MonoBehaviour
{
    [Header("Slider Settings")]
    [SerializeField] private Slider hpBar;

    [Header("Header Settings")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text protectText;

    [Header("Image Settings")]
    [SerializeField] private Image hpIcon;
    [SerializeField] private Image ProtectIcon;
    [SerializeField] private Image ProtectImage;

    private bool isProtected;

    private bool IsProtected
    {
        get => isProtected;
        set
        {
            isProtected = value;
            hpIcon.enabled = !value;
            protectText.enabled = ProtectIcon.enabled = ProtectImage.enabled = value;
        }
    }

    private void UpdateView(int curHp, int maxHp, int protect)
    {
        bool hasProtect = protect > 0;
        
        if (isProtected != hasProtect)
            IsProtected = hasProtect;

        if (hasProtect)
            UpdateProtect(protect);
        else
            UpdateHp(curHp, maxHp);
    }

    private void UpdateHp(int curHp, int maxHp)
    {
        hpBar.value = (float)curHp / maxHp;
        hpText.text = $"{curHp}/{maxHp}";
    }

    private void UpdateProtect(int protect)
    {
        protectText.text = $"{protect}";
    }

    public void Bind(ActorStatus status)
    {
        status.Health.OnUpdate.AddListener(UpdateView);

        IsProtected = false;
        Health health = status.Health;
        UpdateView(health.CurHp, health.MaxHp, health.Block);
    }
}
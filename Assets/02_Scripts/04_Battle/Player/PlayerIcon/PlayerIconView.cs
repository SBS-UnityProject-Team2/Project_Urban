using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerIconView : MonoBehaviour
{
    [Header("Image Settings")]
    [SerializeField] private SpriteAtlas playerIcon;
    [SerializeField] private Image iconImage;

    [Header("Hp Thresholds Setting")]
    [SerializeField] private int[] hpThresholds = new int[] { 100, 70, 40, 0 };

    private readonly Sprite [] sprites = new Sprite[4];

    private void Awake()
    {
        playerIcon.GetSprites(sprites);
        iconImage.sprite = sprites[0];
    }

    private void UpdateIcon(int curHp ,int maxHp, int _)
    {
        int hpRatio = (int)((float)curHp / maxHp * 100.0f);

        for (int i = 0; i < hpThresholds.Length; i++)
        {
            if (hpRatio >= hpThresholds[i])
            {
                iconImage.sprite = sprites[i];
                
                break;
            }
        }
    }

    public void Bind(HealthController health)
    {
        health.OnUpdate.AddListener(UpdateIcon);
        UpdateIcon(health.CurrentHp, health.MaxHp, health.Protect);
    }
}
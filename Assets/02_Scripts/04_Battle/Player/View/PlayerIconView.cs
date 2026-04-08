using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconView : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private List<Sprite> playerIcons;
    [SerializeField] private List<float> changeStep;

    private void UpdateView(int curHp, int maxHp, int block)
    {
        float ratio = (float)curHp / maxHp;
        
        for (int i = changeStep.Count - 1; i >= 0; i--)
        {
            if (ratio <= changeStep[i])
            {
                iconImage.sprite = playerIcons[i];
                return;
            }
        }
    }

    public void Bind(ActorStatus status)
    {
        status.Health.OnUpdate.AddListener(UpdateView);
        
        Health health = status.Health;
        UpdateView(health.CurHp, health.MaxHp, health.Block);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeathView : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image blockImage;

    private bool isBlock;

    public void Bind(ActorStatus status)
    {
        status.Health.OnUpdate.AddListener(UpdateView);

        Health health = status.Health;
        UpdateView(health.CurHp, health.MaxHp, health.Block);
    }

    private void UpdateView(int curHp, int maxHp, int block)
    { 
        if (!isBlock)
        {
            if (block > 0)
            {
                isBlock = true;
                blockImage.enabled = true;
                valueText.text = $"{block}";
            }
            else
            {
                hpSlider.value = (float)curHp / maxHp;
                valueText.text = $"{curHp} / {maxHp}";
            }
        }
        else
        {
            if (block > 0)
            {
                valueText.text = $"{block}";
            }
            else
            {
                isBlock = false;
                blockImage.enabled = false;
                hpSlider.value = (float)curHp / maxHp;
                valueText.text = $"{curHp} / {maxHp}";
            }
        }
    }
}
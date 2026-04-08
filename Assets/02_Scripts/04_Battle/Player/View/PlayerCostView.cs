using TMPro;
using UnityEngine;

public class PlayerCostView : MonoBehaviour
{
    [SerializeField] private TMP_Text costText;

    private void UpdateView(int curCost, int maxCost)
    {
        costText.text = $"{curCost} / {maxCost}";
    }

    public void Bind(ActorStatus status)
    {
        status.Cost.OnUpdate.AddListener(UpdateView);

        Cost cost = status.Cost;
        UpdateView(cost.CurCost, cost.MaxCost);
    }
}
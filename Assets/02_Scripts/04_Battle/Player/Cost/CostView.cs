using TMPro;
using UnityEngine;
public class CostView : MonoBehaviour
{
    [SerializeField] TMP_Text costText;

    public void UpdateView(int curCost, int maxCost)
    {
        costText.text = $"{curCost}/{maxCost}";
    }

    public void Bind(CostController costController)
    {
        costController.OnUpdateCost.AddListener(UpdateView);
        UpdateView(costController.CurrentCost, costController.MaxCost);
    }
}   

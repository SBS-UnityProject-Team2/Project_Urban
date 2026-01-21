using TMPro;
using UnityEngine;

public class CoinView : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    public void UpdateView(int curCoin)
    {
        coinText.text = $"{curCoin}";
    }

    public void Bind(CoinController coinController)
    {
        coinController.OnUpdateCoin.AddListener(UpdateView);
        UpdateView(coinController.CurrentCoin);
    }
}
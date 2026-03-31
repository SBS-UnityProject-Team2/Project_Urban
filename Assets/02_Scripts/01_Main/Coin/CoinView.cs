using TMPro;
using UnityEngine;

public class CoinView : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    private CoinController boundCoinController;

    private void Start()
    {
        Bind(PlayerManager.Instance?.Coin ?? CoinController.Fallback);
    }

    private void OnDestroy()
    {
        boundCoinController?.OnUpdateCoin.RemoveListener(UpdateView);
    }

    public void UpdateView(int curCoin)
    {
        coinText.text = $"{curCoin}";
    }

    public void Bind(CoinController coinController)
    {
        boundCoinController?.OnUpdateCoin.RemoveListener(UpdateView);

        boundCoinController = coinController;
        coinController.OnUpdateCoin.AddListener(UpdateView);
        UpdateView(coinController.CurrentCoin);
    }
}
using UnityEngine.Events;

public class CoinController
{
    private int curCoin;
    public int CurrentCoin => curCoin;

    public UnityEvent<int> OnUpdateCoin { get; } = new();

    public void IncreaseCoin(int amount)
    {
        curCoin += amount;

        OnUpdateCoin?.Invoke(curCoin);
    }

    public void DecreaseCoin(int amount)
    {
        curCoin -= amount;

        if (curCoin < 0)
            curCoin = 0;

        OnUpdateCoin?.Invoke(curCoin);
    }

    public bool CanBuy(int price)
    {
        return curCoin >= price;
    }
}
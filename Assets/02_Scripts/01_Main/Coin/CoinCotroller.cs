using UnityEngine.Events;

public class CoinController
{
    private int curCoin;

    // PlayerManager.Coin이 없을 때 임시 테스트용으로 공유하는 대체 코인 소스
    public static CoinController Fallback { get; } = new CoinController(9999);

    public CoinController(int curCoin)
    {
        this.curCoin = curCoin;
    }

    public int CurrentCoin => curCoin;

    public UnityEvent<int> OnUpdateCoin { get; } = new();

    public void Increase(int amount)
    {
        curCoin += amount;

        OnUpdateCoin?.Invoke(curCoin);
    }

    public void Decrease(int amount)
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
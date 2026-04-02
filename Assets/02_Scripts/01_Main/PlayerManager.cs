using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Player Hp Settings")]
    [SerializeField] private int maxHp = 500;
    
    [Header("Player Coin Settings")]
    [SerializeField] private int curCoin = 9999;

    private HealthController health;
    private CoinController coin;

    public HealthController Health => health;
    public CoinController Coin => coin;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this) return;

        health = new HealthController(maxHp);
        coin = new CoinController(curCoin);
    }
}
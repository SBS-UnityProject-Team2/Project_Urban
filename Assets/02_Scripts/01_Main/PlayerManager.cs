using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Player Hp Settings")]
    [SerializeField] private int maxHp = 500;

    private HealthController health;
    private CoinController coin;

    public HealthController Health => health;
    public CoinController Coin => coin;

    private void Start()
    {
        health = new HealthController(maxHp);
        coin = new CoinController();
    }    
}
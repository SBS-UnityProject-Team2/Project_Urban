using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Player Hp Settings")]
    [SerializeField] private int maxHp = 50;
    
    [Header("Player Coin Settings")]
    [SerializeField] private int curCoin = 9999;

    private HealthController health;
    private CoinController coin;
    private ArtifactController artifacts;

    public HealthController Health => health;
    public CoinController Coin => coin;
    public ArtifactController Artifacts => artifacts;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this) return;

        health = new HealthController(maxHp);
        coin = new CoinController(curCoin);
        artifacts = new ArtifactController();
    }    
}
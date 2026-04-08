using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Player Hp Settings")]
    [SerializeField] private int maxHp = 500;
    
    [Header("Player Coin Settings")]
    [SerializeField] private int curCoin = 9999;

    private HealthController health;
    private CoinController coin;
    private ArtifactManager artifact;
    private readonly List<ArtifactId> ownedArtifacts = new();

    public HealthController Health => health;
    public CoinController Coin => coin;
    public ArtifactManager Artifact => artifact;
    public IReadOnlyList<ArtifactId> OwnedArtifacts => ownedArtifacts;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this) return;

        health = new HealthController(maxHp);
        coin = new CoinController(curCoin);
        artifact = new ArtifactManager();
    }

    public void AddArtifact(ArtifactId artifactId)
    {
        if (ownedArtifacts.Contains(artifactId))
            return;

        ownedArtifacts.Add(artifactId);
    }

    public bool HasArtifact(ArtifactId artifactId)
    {
        return ownedArtifacts.Contains(artifactId);
    }

    public void RemoveArtifact(ArtifactId artifactId)
    {
        ownedArtifacts.Remove(artifactId);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerHeathView heathView;
    [SerializeField] private PlayerCostView costView;
    [SerializeField] private PlayerIconView playerIconView;
    [SerializeField] private PlayerStatusEffectListView statusEffectView;
    [SerializeField] private PlayerArtifactView playerArtifactView;

    public void Init(ActorStatus status, List<Artifact> artifacts)
    {
        heathView.Bind(status);
        costView.Bind(status);
        playerIconView.Bind(status);
        statusEffectView.Bind(status);
        playerArtifactView.Init(artifacts);
    }
}
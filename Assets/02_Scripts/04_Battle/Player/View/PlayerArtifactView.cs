using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerArtifactView : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private PlayerArtifactItemView artifactPrefab;
    
    public void Init(List<Artifact> artifacts)
    {
        foreach (Artifact artifact in artifacts)
        {
            PlayerArtifactItemView art = Instantiate(artifactPrefab, rectTransform);
            art.Init(artifact.Id);
        }
    }
}
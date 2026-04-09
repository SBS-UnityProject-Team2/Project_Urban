using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerArtifactView : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text artifactPrefab;
    
    public void Init(List<Artifact> artifacts)
    {
        foreach (Artifact artifact in artifacts)
        {
            TMP_Text art = Instantiate(artifactPrefab, rectTransform);
            art.text = $"[{artifact.KoreanName}]";
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private StatusEffectData effectData;

    // Battle Stage Score
    private int minScore = 2;
    private int maxScore = 2;
    public bool IsNormal { get; set; } = true;

    public Element SelectedElement { get; set; } = Element.None;

    public void GameOver()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(Scene.Main);
    }

    public void SetEnemyScore(int minScore, int maxScore)
    {
        this.minScore = minScore;
        this.maxScore = maxScore;
    }

    public int GetEnemyScore()
    {
        return Random.Range(minScore, maxScore + 1);
    }

    public StatusEffectDataEntry GetEffectData(StatusEffectName effectName)
    {
        return effectData.GetEffectData(effectName);
    }
}
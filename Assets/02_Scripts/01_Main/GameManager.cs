using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int minScore = 2;
    private int maxScore = 2;

    public bool IsNormal { get; set; } = true;

    public void SetEnemyScore(int minScore, int maxScore)
    {
        this.minScore = minScore;
        this.maxScore = maxScore;
    }

    public int GetEnemyScore()
    {
        return Random.Range(minScore, maxScore + 1);
    }
}
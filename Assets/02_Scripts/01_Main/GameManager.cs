using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Player Hp Settings")]
    [SerializeField] private int playerMaxHp = 500;
    private readonly List<CardName> initialDeck = new();

    private HealthController playerHp;
    private CoinController coin;
    // Battle Stage Score
    private int minScore = 2;
    private int maxScore = 2;
    public bool IsNormal { get; set; } = true;

    public HealthController PlayerHealth => playerHp;
    public CoinController Coin => coin;

    private void Start()
    {
        playerHp = new HealthController(playerMaxHp);
        coin = new CoinController();
    }

    // 카드를 저장하는 함수


    //플레이어가 처음에 선택한 속성 저장(StoreUI 용)
    public Element SelectedElement { get; set; } = Element.None;

    // 메인화면으로 돌아갈 때, 맵 선택창을 띄울지 여부
    // public bool IsStageSelectMode { get; set; } = false;

    public void AddCoin(int amount)
    {
        coin.IncreaseCoin(amount);
    }

    public void UseCoin(int amount)
    {
        coin.DecreaseCoin(amount);
    }
    
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
}
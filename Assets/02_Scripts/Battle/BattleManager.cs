using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SceneSingleton<BattleManager>
{
    // Player
    [SerializeField] private Player player;
    private int curTurn;
    private bool isPlayerTurn = true;
    private bool isBattleEnded = false;
    private bool isBattlePause = false;
    private int earnedCoin;

    public Player Player => player;
    public int CurrentTurn => curTurn;
    public bool IsPlayerTurn => isPlayerTurn;
    public bool IsBattleEnded => isBattleEnded;
    public bool IsBattlePause => isBattlePause;
    public int EarnedCoin => earnedCoin;

    public UnityEvent<bool> OnBattleEnd = new();
    
    private void Start()
    {
        OnBattleEnd.AddListener(HandleBattleEnd);
        InitializeBattle();
    }
    
    private void InitializeBattle()
    {
        player.OnTurnStart?.Invoke();
    }

    private void HandleBattleEnd(bool isVictory)
    {
        if (isBattleEnded) return; // 중복 호출 방지
        
        isBattleEnded = true;
        isPlayerTurn = false;

        player.Hand.RemoveAll();
        
        // 코인 전달
        GameManager.Instance.AddCoin(earnedCoin);
        GameManager.Instance.PlayerHealth.ResetProtect();
    }
        
    private void StartPlayerTurn()
    {
        player.OnTurnStart?.Invoke();
    }

    public void EndPlayerTurn()
    {
        if (!isPlayerTurn) return;

        player.OnTurnEnd?.Invoke();
    
        isPlayerTurn = false;
        curTurn++;

        EnemyManager.Instance.ExecuteEnemyAction(() =>
        {
            isPlayerTurn = true;
            StartPlayerTurn();
        });
    }

    public void Pause()
    {
        isBattlePause = true;
    }

    public void Restart()
    {
        isBattlePause = false;
    }

    public void AddCoin(int amount)
    {
        earnedCoin += amount;
        Debug.Log($"Current Coin : {earnedCoin}");
    }
}

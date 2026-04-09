using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class Battle : SceneSingleton<Battle>
{
    [Header("Object Settings")]
    [SerializeField] private Player player;
    [SerializeField] private Monsters monsters;
    [SerializeField] private Deck deck;
    [SerializeField] private Hand hand;

    [Header("Battle Settings")]
    [SerializeField] private int drawCount;
    [SerializeField] private int monsterScore;
    [SerializeField] private MonsterLevel monsterLevel;
    
    public Player Player => player;
    public Monsters Monsters => monsters;
    public Deck Deck => deck;
    public Hand Hand => hand;

    public int EarnCoin { get; set; }

    public int DrawCount
    {
        get => drawCount;
        set => drawCount = value;
    }

    public bool IsPause { get; set; }

    public UnityEvent OnBattleStart = new();
    public UnityEvent<bool> OnBattleEnd = new();

    private async void Start()
    {
        deck.Init(DeckManager.Instance.Deck, hand);

        int startScore = monsterScore;
        MonsterLevel startLevel = monsterLevel;
        if (MapManager.Instance.DemoMonsterScorePreset(out int presetScore, out MonsterLevel presetLevel))
        {
            startScore = presetScore;
            startLevel = presetLevel;
        }

        await monsters.Init(startScore, startLevel);
        StartBattleLoop();
    }

    async private void StartBattleLoop()
    {
        OnBattleStart?.Invoke();

        while (true)
        {
            // 1. 플레이어 턴 시작
            await player.BeginTurn();

            bool allMonstersDead = await WaitForTurnEndOrAllMonstersDead();
            if (allMonstersDead)
            {
                ResultHPCoin(true);
                OnBattleEnd?.Invoke(true);

                break;
            }

            bool playerDead = await ExecuteMonsterActionsOrPlayerDead();
            if (playerDead)
            {
                ResultHPCoin(false);
                OnBattleEnd?.Invoke(false);

                break;
            }
        }

        IsPause = true;
        PlayerManager.Instance.Health.SetHp(Player.Status.Health.CurHp);
    }

    private void ResultHPCoin(bool isPlayerWin)
    {
        int remainingHealth = player.Status.Health.CurHp;
        int acquiredCoins = isPlayerWin ? EarnCoin : 0;

        PlayerManager.Instance.UpdateBattleResult(remainingHealth, acquiredCoins);
    }

    private async UniTask<bool> WaitForTurnEndOrAllMonstersDead()
    {
        if (monsters.Count <= 0)
            return true;

        var allDeadTcs = new UniTaskCompletionSource();
        void onAllDead() => allDeadTcs.TrySetResult();
        monsters.OnAllMonsterDead.AddListener(onAllDead);

        try
        {
            int winIndex = await UniTask.WhenAny(
                player.WaitForTurnEndAsync(),
                allDeadTcs.Task
            );

            return winIndex == 1;
        }
        finally
        {
            monsters.OnAllMonsterDead.RemoveListener(onAllDead);
        }
    }

    private async UniTask<bool> ExecuteMonsterActionsOrPlayerDead()
    {
        using var cts = new CancellationTokenSource();
        void onPlayerDead(EventPayload _) => cts.Cancel();
        player.EventBus.AddEventListener(ActorEvent.Dead, onPlayerDead);

        try
        {
            await monsters.ExecuteAction(cts.Token);
            return false;
        }
        catch (OperationCanceledException)
        {
            return true;
        }
        finally
        {
            player.EventBus.RemoveEventListener(ActorEvent.Dead, onPlayerDead);
        }
    }
}
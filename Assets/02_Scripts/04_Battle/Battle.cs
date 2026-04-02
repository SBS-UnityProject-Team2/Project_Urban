using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

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

    public int DrawCount
    {
        get => drawCount;
        set => drawCount = value;
    }

    public UnityEvent OnBattleStart = new();
    public UnityEvent OnBattleEnd = new();

    private void Start()
    {
        deck.Init(DeckManager.Instance.Deck, hand);
        monsters.Init(monsterScore, monsterLevel);

        StartBattleLoop();
    }

    async private void StartBattleLoop()
    {
        List<Actor> actors = new() { player };
        actors.AddRange(monsters.List);

        OnBattleStart?.Invoke();

        // WaitForTurnEndAsync은 기본적으로 턴 종료 신호까지 대기한다.
        // 만약 몬스터 전원 사망 혹은 플레이어 사망시, 대기를 취소한다.
        // 대기 취소시, 예외가 발생한다.
        // 즉, 예외를 배틀 종료 신호로 취급해 catch에서 배틀 종료 후 처리를 한다.
        try
        {
            while (true)
            {
                foreach (Actor actor in actors)
                {
                    actor.BeginTurn();

                    await actor.WaitForTurnEndAsync();
                }
            }
        }
        catch(OperationCanceledException)
        {
            OnBattleEnd?.Invoke();
        }
    }
}
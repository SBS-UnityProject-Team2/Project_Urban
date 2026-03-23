using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Battle : SceneSingleton<Battle>
{
    [SerializeField] private Actor player;
    [SerializeField] private List<Actor> monsters;

    private readonly List<Actor> actors = new();

    public Actor Player => player;
    public List<Actor> Monsters => monsters;

    public UnityEvent OnBattleStart = new();
    public UnityEvent OnBattleEnd = new();

    private void Start()
    {
        actors.Add(player);
        actors.AddRange(monsters);

        // 배틀 시작 전 필요한 준비하기
        // 덱 초기화 등등

        StartBattleLoop();
    } 

    async private void StartBattleLoop()
    {
        OnBattleStart?.Invoke();
        
        while (true)
        {
            foreach (Actor actor in actors)
            {
                actor.BeginTurn();
            
                await actor.WaitForTurnEndAsync();

                // 플레이어 사망, 적 전체 사망 시 루프 탈출
            }
        }

        // OnBattleEnd?.Invoke();
    } 
}

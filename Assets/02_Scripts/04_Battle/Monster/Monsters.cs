/*
    1. 몬스터 생성하기
    2. 모든 몬스터가 죽었는지 확인하기
    3. 죽었으면 이벤트 발생시키기
*/

using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    private Monster monsterPrefab;
    private readonly List<Monster> monsters = new();

    public Monster this[int idx] => monsters[idx];
    public int Count => monsters.Count;
    public List<Monster> List => monsters;

    private Monster MonsterPrefab
    {
        get
        {
            if (monsterPrefab == null)
                monsterPrefab = MonsterManager.Instance.GetPrefab();

            return monsterPrefab;
        }
    }

    public void Init(int monsterScore, MonsterLevel monsterLevel)
    {
        if (monsterLevel == MonsterLevel.Normal)
        {
            while (monsterScore > 0)
            {
                int randScore = Random.Range(0, monsterScore + 1);
                bool result = CreateRandomMonster(randScore, out Monster monster);

                if (!result)
                    continue;

                monsterScore -= randScore;
                monsters.Add(monster);
            }
        }

        else
        {
            CreateRandomMonster(monsterScore, out Monster monster);
            monsters.Add(monster);
        }
    }
    
    public void Init(MonsterName monsterName)
    {
        MonsterDataEntry monsterData = MonsterManager.Instance.GetMonsterData(monsterName);
        monsters.Add(CreateMonster(monsterData));
    }

    private bool CreateRandomMonster(int score, out Monster monster)
    {
        List<MonsterDataEntry> monsterList = MonsterManager.Instance.GetMonsterListByScore(score);
        if (monsterList == null)
        {
            monster = null;

            return false;
        }

        monster = CreateMonster(monsterList[Random.Range(0, monsterList.Count)]);

        return true;
    }

    private Monster CreateMonster(MonsterDataEntry monsterDataEntry)
    {
        Monster monster = Instantiate(MonsterPrefab, transform);
        monster.Init(monsterDataEntry);
        monster.EventBus.AddEventListener(ActorEvent.Dead, eventPayload =>
        {
            monsters.Remove(eventPayload.source as Monster);
            Align();
        });

        return monster;
    }

    private async void Align()
    {
        
    }
}
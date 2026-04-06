using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private MonsterActionData monsterActionData;

    public Monster GetPrefab()
    {
        return monsterData.Prefab;
    }

    public MonsterDataEntry GetMonsterData(MonsterName monsterName)
    {
        return monsterData.GetMonsterData(monsterName);
    }

    public Sprite GetMonsterImage(MonsterName monsterName)
    {
        return monsterData.GetMonsterImage(monsterName);
    }

    public MonsterActionDataEntry GetMonsterAction(int actionId)
    {
        return monsterActionData[actionId];
    }

    public List<MonsterDataEntry> GetMonsterListByScore(int score)
    {
        return monsterData.GetMonsterListByScore(score);
    }

    public List<MonsterDataEntry> GetMonsterListByLevel(MonsterLevel level)
    {
        return monsterData.GetMonsterListByLevel(level);
    }
} 
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField] private MonsterData monsterData;

    public Monster GetPrefab()
    {
        return monsterData.Prefab;
    }

    public MonsterDataEntry GetMonsterData(MonsterName monsterName)
    {
        return monsterData.GetMonsterData(monsterName);
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
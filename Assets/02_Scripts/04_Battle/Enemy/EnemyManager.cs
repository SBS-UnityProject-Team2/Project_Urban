using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ActionIcon
{
    public ActionType type;
    public Sprite sprite;
}

public class EnemyManager : SceneSingleton<EnemyManager>
{
    static readonly float variationCoefficient = 0.2f;
    [Header("Enemy Spawn Settings")]
    [SerializeField] private int enemyCount;
    [SerializeField] private float enemySpacing = 3.5f;

    [Header("Enemy Resource Settings")]
    [SerializeField] private List<Enemy> enemyPrefabs = new();
    [SerializeField] private List<ActionIcon> actionIconList;

    private readonly List<Enemy> enemies = new();
    private readonly Dictionary<ActionType, Sprite> actionIconMap = new();

    public List<Enemy> EnemyList => enemies;

    private void Start()
    {
        InitActionIconMap();

        int score = GameManager.Instance.GetEnemyScore();

        if (GameManager.Instance.IsNormal)
            CreateNormalEnemies(score);
        else
            CreateEliteEnemy(score);

        AlignEnemies();
    }

    private void InitActionIconMap()
    {
        foreach (ActionIcon actionIcon in actionIconList)
            actionIconMap[actionIcon.type] = actionIcon.sprite;
    }

    private Enemy CreateEnemy(List<Enemy> enemies)
    {
        int idx = Random.Range(0, enemies.Count);

        Enemy enemy = Instantiate(enemies[idx], transform);
        enemy.OnDead.AddListener(HandleEnemyDead);

        return enemy;
    }

    private void CreateNormalEnemies(int score)
    {
        List<Enemy> filteredEnemies = FilterEnemies(enemy => enemy.Score <= score);

        while (score > 0)
        {
            Enemy enemy = CreateEnemy(filteredEnemies);
            score -= enemy.Score;

            enemies.Add(enemy);
        }
    }

    private void CreateEliteEnemy(int score)
    {
        List<Enemy> filteredEnemies = FilterEnemies(enemy => enemy.Score == score);
        Enemy enemy = CreateEnemy(filteredEnemies);

        enemies.Add(enemy);
    }

    private List<Enemy> FilterEnemies(System.Func<Enemy, bool> where)
    {
        return enemyPrefabs.Where(where).ToList();
    }

    private void AlignEnemies()
    {
        int count = enemies.Count;
        if (count == 0) return;

        float totalWidth = (count - 1) * enemySpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < count; i++)
        {
            Vector3 targetPos = transform.position + new Vector3(startX + (i * enemySpacing), 0, 0);
            enemies[i].MoveTo(targetPos);
        }
    }

    private void HandleEnemyDead(Target target)
    {
        enemies.Remove(target as Enemy);
        Destroy(target.gameObject);
        AlignEnemies();

        BattleManager.Instance.AddCoin(CalcCoin(target as Enemy));

        if (enemies.Count == 0)
            BattleManager.Instance.OnBattleEnd?.Invoke(true);
    }

    private IEnumerator ExecuteEnemyActionRoutine(UnityAction completeRoutine)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];

            // Enemy Attack Animation 
            enemy.OnTurnStart?.Invoke();
            if (!enemy.Status.Frozen.IsActive)
            {
                enemy.Action();

                // Enemy Anim
                enemy.transform.localScale *= 1.2f;
                yield return new WaitForSeconds(0.5f);
                enemy.transform.localScale /= 1.2f;
            }

            enemy.OnTurnEnd?.Invoke();
        }

        completeRoutine?.Invoke();
    }

    private int CalcCoin(Enemy enemy)
    {
        return (int)(10.0f * (1.0f + enemy.RewardCoin) * (1.0f + Random.Range(variationCoefficient * -1.0f, variationCoefficient)));
    }

    public Sprite GetActionIcon(ActionType actionType)
    {
        return actionIconMap[actionType];
    }

    public void ExecuteEnemyAction(UnityAction completeRoutine = null)
    {
        StartCoroutine(ExecuteEnemyActionRoutine(completeRoutine));
    }

    public void DamageAll(int hitPoint, Element attackType = Element.None)
    {
        ApplyAll(enemy => enemy.Damage(BattleManager.Instance.Player, hitPoint, attackType));
    }

    public void ApplyAll(UnityAction<Enemy> action)
    {
        foreach (Enemy enemy in enemies)
            action?.Invoke(enemy);
    }
}
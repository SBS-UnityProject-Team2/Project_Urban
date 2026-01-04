using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

abstract public class Enemy : Target
{   
    [Header("UI Settings")]
    [SerializeField] protected Image selectedArrow;
    [SerializeField] protected NextActionView nextActionText;

    [Header("Enemy Settings")]
    [SerializeField] protected int score;
    [SerializeField] protected int baseHp;
    [SerializeField] private int typeCount;
    [SerializeField] private int hpIncrement;
    [SerializeField] private float rewardCoin;

    [Header("Pattern Settings")]
    [SerializeField] private List<ActionType> actionPattern = new();
    [SerializeField] private List<EnemyAction> enemyActions = new();
    

    private readonly Dictionary<ActionType, List<EnemyAction>> actionMap = new();
    private int actionIdx = 0;
    private EnemyAction enemyAction;
    private Coroutine moveCoroutine;
    private IEnemyEventHandler handler;

    public float RewardCoin => rewardCoin;
    public int Score => score;

    private void Awake()
    {   
        // Action Pattern 초기화
        foreach(ActionType type in actionPattern)
        {
            if (!actionMap.ContainsKey(type))
                actionMap[type] = new List<EnemyAction>();
        }

        foreach(EnemyAction action in enemyActions)
            actionMap[action.Type].Add(action);

        // A B C 타입 랜덤 설정하기
        int increment = Random.Range(0, typeCount) * hpIncrement;
        baseHp += increment;

        Health = new HealthController(baseHp);
    }

    private void Start()
    {   
        // 핸들러 주입
        handler = BattleManager.Instance?.Player;
        
        healthView.Bind(Health);
        selectedArrow.enabled = false;

        SetNextEnemyAction();
    }

    protected void SetNextEnemyAction()
    {   
        List<EnemyAction> actions = actionMap[actionPattern[actionIdx]];
        enemyAction = actions[Random.Range(0, actions.Count)];

        // UI 표시
        nextActionText.SetNextActionText(enemyAction.ToString());
        
        actionIdx = (actionIdx + 1) % actionPattern.Count;
    }

    public void Action()
    {
        if (enemyAction.Type == ActionType.Attack || enemyAction.Type == ActionType.Debuff)
            enemyAction.Execute(BattleManager.Instance.Player);
        else   
            enemyAction.Execute(this);

        SetNextEnemyAction();
    }

    public void Hover()
    {
        selectedArrow.enabled = true;
    }

    public void UnHover()
    {
        selectedArrow.enabled = false;
    }

    // Unity 마우스 이벤트
    private void OnMouseEnter()
    {
        handler?.OnEnemyEnter(this);
    }

    private void OnMouseExit()
    {
        handler?.OnEnemyExit(this);
    }

    private void OnMouseDown()
    {
        handler?.OnEnemyClick(this);
    }

    public void MoveTo(Vector3 targetPos, UnityAction onComplete = null)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveRoutine(targetPos, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, UnityAction onComplete, float duration = 0.5f)
    {
        float time = 0;
        Vector3 startPos = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        moveCoroutine = null;

        onComplete?.Invoke();
    }
}
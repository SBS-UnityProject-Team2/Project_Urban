using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class PatternList
{
    public List<ActionList> actionLists;
}

[Serializable]
public class ActionList
{
    public List<EnemyAction> enemyActions;
}

public class Enemy : Target
{
    [Header("UI Settings")]
    [SerializeField] protected Image selectedArrow;
    [SerializeField] protected NextActionView nextActionView;

    [Header("Enemy Settings")]
    [SerializeField] protected int score;
    [SerializeField] protected int maxHp;

    [Header("Pattern Settings")]
    [SerializeField] private List<PatternList> enemyActions = new();
    [SerializeField] private int phaseChangeHp = -1;

    private int actionIdx = 0;
    private EnemyAction enemyAction;
    private Coroutine moveCoroutine;
    private IEnemyEventHandler handler;

    public float RewardCoin => score * 0.1f;
    public int Score => score;

    private int phase = 0;
    private List<ActionList> curPattern;

    protected override void Awake()
    {
        base.Awake();

        Health = new HealthController(maxHp);
        OnTurnStart.AddListener(() => Health.ResetProtect());
    }

    private void Start()
    {
        // 핸들러 주입
        handler = BattleManager.Instance.Player;

        healthView.Bind(Health);
        selectedArrow.enabled = false;

        curPattern = enemyActions[phase].actionLists;

        OnDamaged.AddListener((_, __, ___) =>
        {
            if (Health.CurrentHp <= phaseChangeHp)
            {
                phase++;
                curPattern = enemyActions[phase].actionLists;
                actionIdx = 0;
                
                SetNextEnemyAction();
            }
        });

        SetNextEnemyAction();
    }

    protected void SetNextEnemyAction()
    {
        ActionList actionList = curPattern[actionIdx];
        enemyAction = actionList.enemyActions[UnityEngine.Random.Range(0, actionList.enemyActions.Count)];
        actionIdx = (actionIdx + 1) % curPattern.Count;

        nextActionView.UpdateNextAction(enemyAction);
    }

    public void Action()
    {
        bool isPlayerTarget = (enemyAction.Type & (ActionType.Attack | ActionType.Debuff)) != 0;
        Target target = isPlayerTarget ? BattleManager.Instance.Player : this;

        enemyAction.Execute(this, target);

        if ((enemyAction.Type & ActionType.Attack) != 0)
            OnAttack?.Invoke(this, target);

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
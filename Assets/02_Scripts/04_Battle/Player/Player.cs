using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Linq;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class Player : Target, ICardEventHandler, IEnemyEventHandler
{
    [Header("Player Settings")]
    [SerializeField] private int maxCost;
    [SerializeField] private int drawCount = 6; 
    [SerializeField] private Deck deck;

    [Header("Player View")]
    [SerializeField] protected CostView costView;

    [Header("UI Reference")]
    [SerializeField] private DiscardPanelUI discardPanelUI;

    private PlayerInput playerInput;
    private PlayerStateMachine stateMachine;
    private int nextTurnDrawBonus = 0; 

    public CostController Cost { get; private set; }
    public PlayerStateMachine StateMachine => stateMachine;
    public Deck Deck => deck;
    public int CurrentHandCount => deck.CurrentHandCount;

    protected override void Awake()
    {
        base.Awake();
        deck.Init();

        InitViews();
        InitEvent();
        
        stateMachine = new PlayerStateMachine(this);
        stateMachine.ChangeState<IdleState>();
    }

    private void InitViews()
    {
        Health = PlayerManager.Instance.Health;
        healthView.Bind(Health);

        Cost = new CostController(maxCost);
        costView.Bind(Cost);
    }

    private void InitEvent()
    {
        OnDead.AddListener(HandleDead);
        OnTurnStart.AddListener(HandleTurnStart);
        OnTurnEnd.AddListener(HandleTurnEnd);

        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["RightClick"].started += OnRightClick;
    }

    private void OnDestroy()
    {
        playerInput.actions["RightClick"].started -= OnRightClick;
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<IdleState>();
    }

    private void StartPlayerCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void HandleTurnStart()
    {
        if (status.Frozen.IsActive)
        {
            BattleManager.Instance.EndPlayerTurn();
            return;
        }

        int totalDraw = drawCount + nextTurnDrawBonus;

        deck.Draw(totalDraw);
        Cost.Recovery();
        Health.ResetProtect();

        nextTurnDrawBonus = 0;
        stateMachine.ChangeState<IdleState>();
    }

    public void HandleTurnEnd()
    {
        stateMachine.ChangeState<IdleState>();
        deck.DiscardAll();
    }

    private void HandleDead(Target target)
    {
        BattleManager.Instance.OnBattleEnd?.Invoke(false);
    }

    public void DrawCard(int amount = 1)
    {
        deck.Draw(amount);
    }

    public void DiscardCard(int minCount, int maxCount, UnityAction<int> onComplete)
    {
        discardPanelUI.OpenPanel(minCount, maxCount);
        stateMachine.ChangeToDiscard(discardPanelUI);
    }

    public void AddNextTurnDrawCount(int amount)
    {
        nextTurnDrawBonus += amount;
    }

    public bool IsEnable()
    {
        if (BattleManager.Instance.IsBattleEnded) return false;
        if (BattleManager.Instance.IsBattlePause) return false;
        if (!BattleManager.Instance.IsPlayerTurn) return false;

        return true;
    }

    // ICardEventHandler 구현 - 현재 상태에 위임
    public void OnCardEnter(Card card)
    {
        stateMachine.CurrentState?.OnCardEnter(this, card);
    }

    public void OnCardExit(Card card)
    {
        stateMachine.CurrentState?.OnCardExit(this, card);
    }

    public void OnCardClick(Card card)
    {
        stateMachine.CurrentState?.OnCardClick(this, card);
    }

    // IEnemyEventHandler 구현 - 현재 상태에 위임
    public void OnEnemyEnter(Enemy enemy)
    {
        stateMachine.CurrentState?.OnEnemyEnter(this, enemy);
    }

    public void OnEnemyExit(Enemy enemy)
    {
        stateMachine.CurrentState?.OnEnemyExit(this, enemy);
    }

    public void OnEnemyClick(Enemy enemy)
    {
        stateMachine.CurrentState?.OnEnemyClick(this, enemy);
    }

    public void UseCard(Card card, Target target)
    {
        int cost = card.Use(this, target);

        Cost.Decrease(cost);
        deck.Use(card);

        if (card.Type == CardType.Attack)
            OnAttack?.Invoke(this, target);
    }

    public void IncreaseDrawCount(int amount = 1)
    {
        drawCount += amount;
    }

    public void DecreaseDrawCount(int amount = 1)
    {
        drawCount -= amount;

        if (drawCount < 0)
            drawCount = 0;
    }

    /// <summary>
    /// 이펙트를 재생하는 코루틴
    /// 이펙트 재생 중에는 카드 선택 불가 상태로 전환
    /// </summary>
    public IEnumerator PlayEffect(EffectType effectType)
    {
        EffectDataEntry effectData = EffectManager.Instance.GetEffectData(effectType);
        
        // 카드 선택 비활성화
        foreach (Card card in deck.Hand.CurHand)
            card.IsDiscardSelect = true;

        int[,] pattern = effectData.effectPattern;
        float[] duration = effectData.effectDuration;
        
        int rowCount = pattern.GetLength(0);
        int colCount = pattern.GetLength(1);
        
        GameObject[] currentEffects = new GameObject[colCount];
        int effectCount = 0;
        GameObject prefab = EffectManager.Instance.GetEffectPrefab(effectType);

        // 이펙트 생성 및 이동
        for (int i = 0; i < rowCount; i++)
        {
            if (i == 0)
            {
                // 첫 번째 단계: 이펙트 생성
                for (int j = 0; j < colCount; j++)
                {
                    currentEffects[j] = Instantiate(
                        prefab,
                        EffectManager.Instance.GetPositionByIndex(pattern[i, j]),
                        Quaternion.identity
                    );
                }
                effectCount = colCount;
            }
            else
            {
                // 다음 단계들: 이펙트 이동
                for (int j = 0; j < effectCount; j++)
                    currentEffects[j].transform.position = 
                        EffectManager.Instance.GetPositionByIndex(pattern[i, j]);
            }

            // 대기
            if (i < duration.Length)
                yield return new WaitForSeconds(duration[i]);
        }

        // 이펙트 소멸
        for (int i = 0; i < effectCount; i++) Destroy(currentEffects[i]);

        // 카드 선택 활성화
        foreach (Card card in deck.Hand.CurHand) card.IsDiscardSelect = false;
    }
}

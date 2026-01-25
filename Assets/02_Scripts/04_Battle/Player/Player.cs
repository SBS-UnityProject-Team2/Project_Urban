using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(PlayerInput))]
public class Player : Target, ICardEventHandler, IEnemyEventHandler
{
    [Header("Player Settings")]
    [SerializeField] private int maxCost;
    [SerializeField] private int drawCount = 6; 
    [SerializeField] private CardSystem cardSystem;

    [Header("Player View")]
    [SerializeField] protected CostView costView;

    private PlayerInput playerInput;
    private PlayerStateMachine stateMachine;
    private int nextTurnDrawBonus = 0; 

    public CostController Cost { get; private set; }
    public PlayerStateMachine StateMachine => stateMachine;
    public CardSystem CardSystem => cardSystem;
    public int CurrentHandCount => cardSystem.Hand.CurHand.Count();

    protected override void Awake()
    {
        base.Awake();
        cardSystem.Init();

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

    public void HandleTurnStart()
    {
        if (status.Frozen.IsActive)
        {
            BattleManager.Instance.EndPlayerTurn();
            return;
        }

        int totalDraw = drawCount + nextTurnDrawBonus;

        cardSystem.Draw(totalDraw);
        Cost.Recovery();
        Health.ResetProtect();

        nextTurnDrawBonus = 0;
        stateMachine.ChangeState<IdleState>();
    }

    public void HandleTurnEnd()
    {
        stateMachine.ChangeState<IdleState>();
        cardSystem.DiscardAll();
    }

    private void HandleDead(Target target)
    {
        BattleManager.Instance.OnBattleEnd?.Invoke(false);
    }

    public void DrawCard(int amount = 1)
    {
        cardSystem.Draw(amount);
    }

    public void DiscardCard(int minCount, int maxCount, UnityAction<int> onComplete = null)
    {
        stateMachine.ChangeToDiscard(cardSystem.DiscardPanelUI);

        cardSystem.OpenDiscardPanel(minCount, maxCount, count =>
        {
            onComplete?.Invoke(count);
            stateMachine.ChangeState<IdleState>();
        });
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
        cardSystem.Discard(card);

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

    #region Test Methods
    public void TestReinforce()
    {
        int amount = Random.Range(1, 5);
        status.Reinforce.IncreaseStack(amount);
        Debug.Log($"Reinforce 버프 추가: {amount}");
    }

    public void TestArmor()
    {
        int amount = Random.Range(5, 20);
        status.Armor.IncreaseStack(amount);
        Debug.Log($"Armor 버프 추가: {amount}");
    }

    public void TestDummy()
    {
        int amount = Random.Range(1, 3);
        status.Blur.IncreaseStack(amount);
        Debug.Log($"Dummy 버프 추가: {amount}");
    }

    public void TestRefined()
    {
        int turns = Random.Range(1, 4);
        Refined(turns);
        Debug.Log($"Refined 버프 적용: {turns}턴");
    }

    public void TestIncendiary()
    {
        int count = Random.Range(5, 15);
        LoadedIncendiary(count);
        Debug.Log($"Incendiary 버프 활성화: {count}");
    }

    public void TestKineticVeil()
    {
        int turns = Random.Range(1, 3);
        KineticVeil(turns);
        Debug.Log($"KineticVeil 버프 적용: {turns}턴");
    }

    public void TestSuperConduct()
    {
        int turns = Random.Range(1, 3);
        Nullification(turns);
        Debug.Log($"SuperConduct 버프 적용: {turns}턴");
    }

    public void TestBioActiveShell()
    {
        int turns = Random.Range(1, 4);
        status.BioActiveShell.Apply(turns);
        Debug.Log($"BioActiveShell 버프 적용: {turns}턴");
    }

    public void TestRegeneration()
    {
        int turns = Random.Range(2, 5);
        status.Regeneration.Apply(turns);
        Debug.Log($"Regeneration 버프 적용: {turns}턴");
    }

    public void TestSpike()
    {
        int count = Random.Range(5, 20);
        status.Spike.Active(count);
        Debug.Log($"Spike 버프 활성화: {count}");
    }

    public void TestWeaken()
    {
        int amount = Random.Range(1, 5);
        status.Weaken.Apply(amount);
        Debug.Log($"Weaken 디버프 적용: {amount}");
    }

    public void TestBroken()
    {
        int turns = Random.Range(1, 3);
        status.Broken.Apply(turns);
        Debug.Log($"Broken 디버프 적용: {turns}턴");
    }

    public void TestBleed()
    {
        int amount = Random.Range(5, 15);
        status.Bleed.IncreaseStack(amount);
        Debug.Log($"Bleed 디버프 추가: {amount}");
    }

    public void TestBurn()
    {
        int turns = Random.Range(2, 5);
        status.Burn.Apply(turns);
        Debug.Log($"Burn 디버프 적용: {turns}턴");
    }

    public void TestPoisoned()
    {
        int turns = Random.Range(2, 5);
        status.Poisoned.Apply(turns);
        Debug.Log($"Poisoned 디버프 적용");
    }

    public void TestStigma()
    {
        int count = Random.Range(5, 15);
        status.Branded.Active(count);
        Debug.Log($"Stigma 디버프 활성화: {count}");
    }

    public void TestFrozen()
    {
        int turns = Random.Range(1, 3);
        status.Frozen.Apply(turns);
        Debug.Log($"Frozen 디버프 적용: {turns}턴");
    }

    public void TestAnointed()
    {
        int turns = Random.Range(1, 3);
        status.Anointed.Apply(turns);
        Debug.Log($"Anointed 디버프 적용: {turns}턴");
    }

    public void DiscardSelectedCard()
    {
        // 상태 패턴으로 변경되어 이 메서드는 더 이상 사용되지 않음
    }

    public void TestDelirium()
    {
        int turns = Random.Range(1, 4);
        status.Delirium.Apply(turns);
        Debug.Log($"Delirium 디버프 적용: {turns}턴");
    }

    public void TestInfested()
    {
        int turns = Random.Range(2, 5);
        status.Infested.Apply(turns);
        Debug.Log($"Infested 디버프 적용: {turns}턴");
    }

    public void TestScarred()
    {
        int count = Random.Range(1, 4);
        status.Scarred.Active(count);
        Debug.Log($"Scarred 디버프 활성화: {count}");
    }

    public void TestIncreaseAttack()
    {
        int amount = Random.Range(1, 5);
        status.IncreaseAttack(amount);
        Debug.Log($"Attack 증가: {amount}");
    }

    public void TestDecreaseAttack()
    {
        int amount = Random.Range(1, 3);
        status.DecreaseAttack(amount);
        Debug.Log($"Attack 감소: {amount}");
    }
    #endregion
}

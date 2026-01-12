using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : Target, ICardEventHandler, IEnemyEventHandler
{
    [Header("Player Settings")]
    [SerializeField] private int maxCost;
    [SerializeField] private int drawCount = 6; //시작할때 카드 6장 드로우
    [SerializeField] private int nextTurnDrawBonus = 0; // 다음턴에 부여될  추가드로우 수
    [SerializeField] private Hand hand;

    [Header("Player View")]
    [SerializeField] protected CostView costView;

    private PlayerInput playerInput;
    private Deck deck;
    private Card selectedCard;
    private ElectricField electricFieldBuff;         // 전자기장버프 로직용
    private Cinder cinderBuff;                       // 잔불 버프 로직용
    private AccelConcoction accelConcoctionBuff;     // 가속화합물 버프용

    public CostController Cost { get; private set; }

    public int CurrentHandCount => hand.transform.childCount;   // 현재 핸드에 있는 카드 수 확인용
    public Deck Deck
    {
        get
        {
            if (deck == null)
            {
                deck = GameManager.Instance.Deck;
                deck.Initialize(hand);
            }

            return deck;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();

        Health = GameManager.Instance.PlayerHealth;
        Cost = new CostController(maxCost);

        // 죽으면 종료처리
        OnDead.AddListener(HandleDead);
        OnTurnStart.AddListener(HandleTurnStart);
        OnTurnEnd.AddListener(HandleTurnEnd);

        // 우클릭으로 선택 해제
        playerInput.actions["RightClick"].started += OnRightClick;
    }

    private void OnDestroy()
    {
        playerInput.actions["RightClick"].started -= OnRightClick;
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        DeselectCard();
    }

    private void Start()
    {
        healthView.Bind(Health);
        costView.Bind(Cost);
        electricFieldBuff = new ElectricField(this);        //전자기장 버프 로직용
        cinderBuff = new Cinder(this);                      //잔불 버프 로직용
        accelConcoctionBuff = new AccelConcoction(this);    //가속화합물 로직용
    }

    public void HandleTurnStart()
    {
        if (status.IsFrozen)
        {
            BattleManager.Instance.EndPlayerTurn();
            return;
        }

        Cost.Recovery();

        int totalDraw = drawCount + nextTurnDrawBonus;
        Deck.Draw(totalDraw);

        nextTurnDrawBonus = 0;  //추가드로우 후 드로우보너스 초기화(1턴만인경우)
        DeselectCard();
    }

    public void HandleTurnEnd()
    {
        DeselectCard();
        Deck.DiscardAll();
    }

    private void HandleDead(Target target)
    {
        BattleManager.Instance.OnBattleEnd?.Invoke(false);
    }

    public void DrawCard(int amount = 1)
    {
        Deck.Draw(amount);
    }

    // 다음턴 (1턴만) 추가드로우 보너스
    public void AddNextTurnDrawCount(int amount)
    {
        nextTurnDrawBonus += amount;
    }

    // ICardEventHandler 구현
    public void OnCardEnter(Card card)
    {
        if (BattleManager.Instance.IsBattleEnded) return;
        if (!BattleManager.Instance.IsPlayerTurn) return;
        if (selectedCard != null) return;  // 카드가 선택되어 있으면 호버 안 함

        card.Select();
    }

    public void OnCardExit(Card card)
    {
        if (BattleManager.Instance.IsBattleEnded) return;
        if (!BattleManager.Instance.IsPlayerTurn) return;
        if (selectedCard != null) return;  // 카드가 선택되어 있으면 언호버 안 함

        card.UnSelect();
    }

    public void OnCardClick(Card card)
    {
        if (BattleManager.Instance.IsBattleEnded) return;
        if (!BattleManager.Instance.IsPlayerTurn) return;

        // 같은 카드 재클릭 - 자신에게 사용
        if (selectedCard == card)
        {
            TryUseCardOnSelf(card);
            return;
        }

        // 다른 카드 선택
        if (selectedCard != null)
            selectedCard.UnSelect();

        selectedCard = card;
        selectedCard.Select();
    }

    // IEnemyEventHandler 구현
    public void OnEnemyEnter(Enemy enemy)
    {
        if (BattleManager.Instance.IsBattleEnded) return;
        if (!BattleManager.Instance.IsPlayerTurn) return;

        // 카드가 선택되어 있고, 공격/디버프 카드일 때만 호버
        if (selectedCard != null &&
            (selectedCard.Type == CardType.Attack || selectedCard.Type == CardType.Debuff))
        {
            enemy.Hover();
        }
    }

    public void OnEnemyExit(Enemy enemy)
    {
        if (BattleManager.Instance.IsBattleEnded) return;
        if (!BattleManager.Instance.IsPlayerTurn) return;
        enemy.UnHover();
    }

    public void OnEnemyClick(Enemy enemy)
    {
        if (BattleManager.Instance.IsBattleEnded) return;
        if (!BattleManager.Instance.IsPlayerTurn) return;
        if (selectedCard == null) return;

        if (selectedCard.Type != CardType.Attack && selectedCard.Type != CardType.Debuff)
            return;

        if (selectedCard.Cost > Cost.CurrentCost)
        {
            Debug.Log($"코스트 부족: {selectedCard.Cost}/{Cost.CurrentCost}");
            return;
        }

        UseCard(selectedCard, enemy);
    }

    public void DeselectCard()
    {
        if (selectedCard != null)
            selectedCard.UnSelect();

        selectedCard = null;
    }

    private void TryUseCardOnSelf(Card card)
    {
        if (card.Type != CardType.Defense && card.Type != CardType.BuffCard)
            return;

        if (card.Cost > Cost.CurrentCost)
        {
            Debug.Log($"코스트 부족: {card.Cost}/{Cost.CurrentCost}");
            return;
        }

        UseCard(card, this);
    }


    private void UseCard(Card card, Target target)
    {
        int cost = card.Use(target);
        Cost.Decrease(cost);
        Deck.Discard(card);

        selectedCard = null;
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

    // 카드에서 전자기장 버프를 활성화시키는 함수
    public void ActivateElectricField(int damageAmount)
    {        
        electricFieldBuff = new ElectricField(this);
        electricFieldBuff.Active(damageAmount);
    }

    // 잔불버프 활성화 함수
    public void ActivateCinder(int drawAmount)
    {
        cinderBuff.Active(drawAmount);        
    }

    // 가속화합물버프 활성화 함수
    public void ActivateAccelConcoction(int turns)
    {
        accelConcoctionBuff.Apply(turns);
    }
}

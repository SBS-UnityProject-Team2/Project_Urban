using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : Target, ICardEventHandler, IEnemyEventHandler
{
    [Header("Player Settings")]
    [SerializeField] private int maxCost;
    [SerializeField] private int startingDrawCount = 6; //시작할때 카드 6장 드로우
    [SerializeField] private Hand hand;

    [Header("Player View")]
    [SerializeField] protected CostView costView;
    
    private PlayerInput playerInput;
    private Deck deck;
    private Card selectedCard;

    public CostController Cost { get; private set; }
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
    }

    public void HandleTurnStart()
    {
        if (isStun)
        {
            BattleManager.Instance.EndPlayerTurn();
            return;
        }

        Cost.ResetCost();
        Deck.Draw(startingDrawCount);
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

    public void DrawCard(int amount)
    {
        Deck.Draw(amount);
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
        if (card.Type != CardType.Defense && card.Type != CardType.Buff)
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
        Cost.DecreaseCost(cost);
        Deck.Discard(card);

        selectedCard = null;
    }
}

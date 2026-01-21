using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

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

    [Header("UI Reference")]
    [SerializeField] private DiscardPanelUI discardPanelUI;

    private PlayerInput playerInput;
    private Deck deck;
    private Card selectedCard;
    private bool isDiscardMode = false;              // 카드 버리기 로직용

    public CostController Cost { get; private set; }
    public Deck Deck => deck;
    public Hand Hand => hand;

    public int CurrentHandCount => hand.CurHand.Count();


    protected override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();

        deck = DeckManager.Instance.GetDeck(hand);

        Health = GameManager.Instance.PlayerHealth;
        Cost = new CostController(maxCost);;

        OnDead.AddListener(HandleDead);
        OnTurnStart.AddListener(HandleTurnStart);
        OnTurnEnd.AddListener(HandleTurnEnd);

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
        if (status.Frozen.IsActive)
        {
            BattleManager.Instance.EndPlayerTurn();
            return;
        }

        int totalDraw = drawCount + nextTurnDrawBonus;

        deck.Draw(totalDraw);
        Cost.Recovery();

        nextTurnDrawBonus = 0;  //추가드로우 후 드로우보너스 초기화(1턴만인경우)
        DeselectCard();
    }

    public void HandleTurnEnd()
    {
        DeselectCard();
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

    public void DiscardCard(int minCount, int maxCount, UnityAction<int> onComplete = null)    // 자원교환 버프에서 호출용
    {
        isDiscardMode = true;
        DeselectCard();

        discardPanelUI.OpenPanel(minCount, maxCount);
        discardPanelUI.OnConfirm.AddListener(discardList =>
        {
            foreach (Card card in discardList)
                hand.RemoveCard(card);
            
            onComplete?.Invoke(discardList.Count);
            discardPanelUI.OnConfirm.RemoveAllListeners();
            discardPanelUI.ClosePanel();

            isDiscardMode = false;
        });
    }

    // 다음턴 (1턴만) 추가드로우 보너스
    public void AddNextTurnDrawCount(int amount)
    {
        nextTurnDrawBonus += amount;
    }

    private bool IsEnable()
    {
        if (BattleManager.Instance.IsBattleEnded) return false;
        if (BattleManager.Instance.IsBattlePause) return false;
        if (!BattleManager.Instance.IsPlayerTurn) return false;

        return true;
    }

    // ICardEventHandler 구현
    public void OnCardEnter(Card card)
    {
        if (!IsEnable()) return;
        if (selectedCard != null) return;  // 카드가 선택되어 있으면 호버 안 함

        card.Select();
    }

    public void OnCardExit(Card card)
    {
        if (!IsEnable()) return;
        if (selectedCard != null) return;  // 카드가 선택되어 있으면 언호버 안 함

        card.UnSelect();
    }

    public void OnCardClick(Card card)
    {
        if (!IsEnable()) return;

        if (isDiscardMode)
        {
            discardPanelUI.AddCard(card);
            card.UnSelect();

            return;
        }

        if (selectedCard == card)
        {
            TryUseCardOnSelf(card);
            return;
        }

        if (selectedCard != null)
            selectedCard.UnSelect();

        selectedCard = card;
        selectedCard.Select();
    }

    // IEnemyEventHandler 구현
    public void OnEnemyEnter(Enemy enemy)
    {
        if (!IsEnable()) return;

        // 카드가 선택되어 있고, 공격/디버프 카드일 때만 호버
        if (selectedCard != null &&
            (selectedCard.Type == CardType.Attack || selectedCard.Type == CardType.Debuff))
        {
            enemy.Hover();
        }
    }

    public void OnEnemyExit(Enemy enemy)
    {
        if (!IsEnable()) return;
        enemy.UnHover();
    }

    public void OnEnemyClick(Enemy enemy)
    {
        if (!IsEnable()) return;
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
        int cost = card.Use(this, target);

        Cost.Decrease(cost);
        deck.Discard(card);

        if (card.Type == CardType.Attack)
            OnAttack?.Invoke(this, target);

        selectedCard.UnSelect();
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

    #region Test Methods
    // 버프 테스트 메서드들
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

    // 디버프 테스트 메서드들
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
        string cardName = selectedCard.Name.ToString();

        // 2. 선택된 카드를 덱의 Discard 함수로 전달
        deck.Discard(selectedCard);

        // 3. 선택 변수 초기화
        selectedCard = null;
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

    // Attack 테스트
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

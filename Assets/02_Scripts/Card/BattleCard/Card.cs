using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CardView))]        // 카드 UI 담당
[RequireComponent(typeof(CardAction))]      // 카드 동작담당
[RequireComponent(typeof(CardController))]  // 카드 조작 담당
[RequireComponent(typeof(CardEffect))]      // 카드 이펙트
[RequireComponent(typeof(CardState))]       // 카드 상태 관리
public class Card : MonoBehaviour
{
    private static int id = 0;

    private DeckCard deckCard;
    private CardDataEntry cardDataEntry;
    private int cardId;

    // 모듈
    private CardView cardView;
    private CardAction cardAction;    
    private CardEffect cardEffect;
    private CardState cardState;
    private CardController cardController;
    
    public DeckCard DeckCard => deckCard;
    public CardDataEntry CardData => cardDataEntry;
    public CardController Controller => cardController;
    public int Id => cardId;


    private void Awake()
    {
        cardView = GetComponent<CardView>();
        cardAction = GetComponent<CardAction>();
        cardEffect = GetComponent<CardEffect>();
        cardState = GetComponent<CardState>();
        cardController = GetComponent<CardController>();
    }

    public void Init(DeckCard deckCard)
    {        
        cardId = id++;
        this.deckCard = deckCard;

        cardDataEntry = deckCard.CardData;
        InitModules(cardDataEntry);
        cardController.Init(Use);
    }

    public void SetCost(int costPoint)
    {
        cardState.Cost = costPoint;
    }

    public void AddCost(int delta)
    {
        cardState.Cost += delta;
    }

    public void ReduceCost(int delta)
    {
        cardState.Cost -= delta;
    }
    
    public void ResetCost()
    {
        cardState.ResetCost();
    }

    public void Transform(CardName cardName)
    {
        cardDataEntry = CardManager.Instance.GetCardData(cardName);
        InitModules(cardDataEntry);
    }

    public void ResetTransform()
    {
        cardDataEntry = DeckCard.CardData;
        InitModules(cardDataEntry);
    }

    private void InitModules(CardDataEntry cardDataEntry)
    {
        cardView.Init(cardDataEntry);
        cardAction.Init(cardDataEntry.linkId);
        cardEffect.Init(cardDataEntry.effectType);
        cardState.Init(cardDataEntry);
    }

    public async UniTask Use(Actor target)
    {
        CardMethods.Dispatch(cardDataEntry.cardName, target, deckCard.IsEnchanted);
    }    
}
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CardView))]        // 카드 UI 담당
[RequireComponent(typeof(CardAction))]      // 카드 동작담당
[RequireComponent(typeof(CardController))]  // 카드 조작 담당
[RequireComponent(typeof(CardEffect))]      // 카드 이펙트
public class Card : MonoBehaviour
{
    private static int id = 0;

    private DeckCard deckCard;
    private CardDataEntry cardDataEntry;
    private int cardId;

    // 코스트 상태 관리
    private int originCost;
    private int curCost;

    // 모듈
    private CardView cardView;
    private CardAction cardAction;    
    private CardEffect cardEffect;
    private CardController cardController;
    
    public DeckCard DeckCard => deckCard;
    public CardDataEntry CardData => cardDataEntry;
    public int Id => cardId;

    public int Cost
    {
        get => curCost;
        set
        {
            curCost = value;

            if (value < 0)
                curCost = 0;
        }
    }

    private void Awake()
    {
        cardView = GetComponent<CardView>();
        cardAction = GetComponent<CardAction>();
        cardEffect = GetComponent<CardEffect>();
        cardController = GetComponent<CardController>();
    }

    public void Init(DeckCard deckCard)
    {        
        cardId = id++;
        this.deckCard = deckCard;

        cardDataEntry = deckCard.CardData;
        cardView.Init(cardDataEntry);
        cardAction.Init(cardDataEntry.linkId);
        cardEffect.Init(cardDataEntry.effectType);
        cardController.Init(Use);

        originCost = cardDataEntry.cost;
        curCost = originCost;
    }

    public void ResetCost()
    {
        curCost = originCost;
    }

    private async UniTask Use(Actor target)
    {
        IEnumerator seq = cardAction.Execute(target).GetEnumerator();

        while (seq.MoveNext())
        {   
            if (seq.Current is int seqNum && seqNum == 1) 
                await cardEffect.Play();
        }
    }
}
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CardView))]        // 카드 UI 담당
[RequireComponent(typeof(CardAction))]      // 카드 동작담당
[RequireComponent(typeof(CardController))]  // 카드 조작 담당
[RequireComponent(typeof(CardEffect))]      // 카드 이펙트
public class Card : MonoBehaviour, ICardInstance
{
    // 데이터
    private CardInstance cardInstance;

    // 코스트 상태 관리
    private int originCost;
    private int curCost;

    // 모듈
    private CardView cardIView;
    private CardAction cardAction;    
    private CardEffect cardEffect;
    private CardController cardController;
    
    public CardInstance CardInstance => cardInstance;
    public CardDataEntry CardData => cardInstance.CardData;

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
        cardIView = GetComponent<CardView>();
        cardAction = GetComponent<CardAction>();
        cardEffect = GetComponent<CardEffect>();
        cardController = GetComponent<CardController>();
    }

    public void Init(CardInstance cardInstance)
    {
        this.cardInstance = cardInstance;
        
        cardIView.Init(cardInstance);
        cardAction.Init(cardInstance.CardData.linkId);
        cardEffect.Init(cardInstance.CardData.effectType);
        cardController.Init(Use);

        originCost = cardInstance.CardData.cost;
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
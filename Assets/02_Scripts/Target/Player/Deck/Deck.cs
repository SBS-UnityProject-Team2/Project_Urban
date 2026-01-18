using UnityEngine;
using System.Collections.Generic;
using System.Linq; // 리스트 검색(Find) 등을 위해 필요

public class Deck 
{
    // [변경점] List<CardName> -> List<DeckCard>
    // 이제 덱에서는 이름 + 강화 여부가 포함된 객체 목록으로 관리
    private readonly List<DeckCard> originCardList = new();     // 원본 덱
    private readonly List<DeckCard> unusedCardList = new();     // 뽑을 덱 
    private readonly List<DeckCard> usedCardList = new();       // 사용한 카드 리스트 
    private readonly List<DeckCard> extinctCardList = new();    // 소멸된 카드 리스트
    private readonly List<DeckCard> tempCardList = new(12);     

    private Hand hand;
    
    // 외부 접근용 프로퍼티도 DeckCard 타입으로 변경
    public IEnumerable<DeckCard> CardList => originCardList;
    public IEnumerable<DeckCard> UsedCardList => usedCardList;
    public IEnumerable<DeckCard> ExtinctCardList => extinctCardList;

    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;
    public int ExtinctCardCount => extinctCardList.Count;

    public Deck(IEnumerable<CardName> cardRecipes)
    {
        foreach (CardName name in cardRecipes)
        {
            // 최초 생성 시에는 강화되지 않은 상태(false)로 객체 생성
            DeckCard newCard = new DeckCard(name, false);

            originCardList.Add(newCard);
            unusedCardList.Add(newCard); 
        }

        Shuffle();
    }
    
    public void Initialize(Hand hand)
    {
        this.hand = hand;
    }

    // 덱 셔플 로직
    public void Shuffle()
    {
        // 버린 카드 더미가 있다면 뽑을 덱으로 합침
        if (usedCardList.Count > 0)
        {
            unusedCardList.AddRange(usedCardList);
            usedCardList.Clear();
        }

        // Fisher-Yates 알고리즘으로 섞기
        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            // 튜플 분해를 이용한 스왑
            (unusedCardList[rand], unusedCardList[i]) = (unusedCardList[i], unusedCardList[rand]);
        }
    }

    // 카드 여러 장 드로우
    public void Draw(int amount)
    {
        tempCardList.Clear();
        
        for (int i = 0; i < amount; i++)
        {
            // 뽑을 카드가 없으면 셔플
            if (unusedCardList.Count == 0)
                Shuffle();

            // 셔플 후에도 카드가 있으면 뽑기
            if (unusedCardList.Count > 0)
            {
                tempCardList.Add(GetNextCard());
            }
        }
        
        hand.AddCards(tempCardList);
    }

    // 카드 1장 드로우
    public void Draw()
    {
        if (unusedCardList.Count == 0)
            Shuffle();

        if (unusedCardList.Count > 0)
        {
            hand.AddCard(GetNextCard());
        }
    }

    // 다음 카드 가져오기 
    private DeckCard GetNextCard()
    {
        // 리스트의 맨 뒤에서부터 가져옴 
        DeckCard card = unusedCardList[^1];
        unusedCardList.RemoveAt(unusedCardList.Count - 1);

        return card;
    }

    // 사용한 카드 버리기
    public void Discard(Card usedCard)
    {
        // 카드에서 정보(이름, 강화여부)를 확인한 후 DeckCard 생성
        DeckCard cardData = new DeckCard(usedCard.Name, usedCard.IsEnchanted);

        // 1. 소멸(Extinct) 체크
        if (usedCard.IsExtinct)
        {
            extinctCardList.Add(cardData);
        }
        else
        {
            usedCardList.Add(cardData);
        }
        
        // 핸드에서 UI 오브젝트 제거
        hand.RemoveCard(usedCard);
    }

    // 핸드에 있는 모든 카드 버리기
    public void DiscardAll()
    {
        foreach (Card card in hand.CurHand)
        {
            bool isEnchanted = false; 
            isEnchanted = card.IsEnchanted;

            usedCardList.Add(new DeckCard(card.Name, card.IsEnchanted));
        }

        hand.RemoveAll();
    }

    // 전투 종료 후 덱 초기화 (원본 덱 상태로 복구)
    public void ResetDeck()
    {
        unusedCardList.Clear();
        usedCardList.Clear();
        extinctCardList.Clear(); 

        foreach (var card in originCardList)
            unusedCardList.Add(card);

        Shuffle();
    }

    // 카드 강화 로직
    public void UpgradeCard(CardName targetName)
    {
        // 원본 덱에서 해당 이름을 가진 카드 중, 아직 강화되지 않은 카드 검색
        DeckCard targetCard = originCardList.Find(c => c.CardName == targetName && !c.IsEnchanted);

        if (targetCard != null)
        {
            // 상태 변경 -> 참조 타입이므로 originCardList 내부의 객체가 변경됨
            targetCard.IsEnchanted = true;
        }        
    }

    // 덱에 새로운 카드 추가
    public void AddCard(CardName cardName, bool isEnchanted = false)
    {
        DeckCard newCard = new DeckCard(cardName, isEnchanted);
        originCardList.Add(newCard);
        
        // 전투 중이라면 뽑을 덱에도 넣어줌 
        unusedCardList.Add(newCard);
    }

    // 덱에서 카드 영구 제거 (이벤트 등)
    public void RemoveCard(CardName cardName)
    {
        // 강화 안 된 카드 우선 제거, 없으면 강화된 카드 제거
        DeckCard target = originCardList.Find(c => c.CardName == cardName && !c.IsEnchanted);
        
        if (target == null)
            target = originCardList.Find(c => c.CardName == cardName);

        if (target != null)
        {
            originCardList.Remove(target);
            
            if(unusedCardList.Contains(target)) unusedCardList.Remove(target);
            else if(usedCardList.Contains(target)) usedCardList.Remove(target);
        }
    }
    
    // 전체 덱 목록 반환
    public List<DeckCard> GetAllCards()
    {
        return new List<DeckCard>(originCardList);
    }

    // 버린 카드 더미에서 랜덤 뽑기 
    public Card DrawRandomFromDiscard()
    {       
        if (usedCardList.Count == 0) return null;

        int randomIndex = Random.Range(0, usedCardList.Count);
        DeckCard targetDeckCard = usedCardList[randomIndex];

        usedCardList.RemoveAt(randomIndex);

        // Hand에 DeckCard 정보를 넘김
        return hand.AddCard(targetDeckCard);
    }

    // 가장 최근에 사용한 카드 정보 확인
    public DeckCard GetLastUsedCard()
    {
        if (usedCardList.Count > 0)
        {
            return usedCardList[usedCardList.Count - 1];
        }
        return null; 
    }

    // 뽑을 덱 중간에 카드 찔러 넣기
    public void AddCardToDrawPile(CardName cardName, bool isEnchanted = false)
    {
        DeckCard newCard = new DeckCard(cardName, isEnchanted);
        
        int randomIndex = Random.Range(0, unusedCardList.Count + 1);
        unusedCardList.Insert(randomIndex, newCard);
    }

    // 소멸 로직
    public void Extinct(Card card)
    {        
        DeckCard deckCard = new DeckCard(card.Name, card.IsEnchanted);
        extinctCardList.Add(deckCard);
        
        hand.RemoveCard(card);        
    }
    

    // 덱 관리용 카드 데이터 객체
    [System.Serializable]
    public class DeckCard
    {
        public CardName CardName;
        public bool IsEnchanted; // 강화 여부

        public DeckCard(CardName name, bool isEnchanted = false)
        {
            this.CardName = name;
            this.IsEnchanted = isEnchanted;
        }
    }
}
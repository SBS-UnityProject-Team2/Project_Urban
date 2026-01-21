using UnityEngine;
using System.Collections.Generic;

public class ProtoTypeDeck : MonoBehaviour
{
    public static ProtoTypeDeck Instance;

    private List<CardDataEntry> runtimeDeck = new List<CardDataEntry>();

    [System.Serializable]
    public struct DeckRecipe
    {
        public CardName cardName;
        public int count;
    }

    [Header("카드 데이터베이스")]
    [SerializeField] private CardData cardDatabase;

    [Header("초기 덱 구성")]
    [SerializeField] private List<DeckRecipe> deckConfig = new List<DeckRecipe>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // 게임 시작 시 초기 덱을 생성
        InitializeDeck();
    }

    // 1. 초기 덱 생성
    private void InitializeDeck()
    {
        runtimeDeck.Clear();

        foreach (DeckRecipe recipe in deckConfig)
        {
            // 데이터베이스에서 카드 정보 가져오기            
            CardDataEntry entry = cardDatabase.GetCardData(recipe.cardName);


            for (int i = 0; i < recipe.count; i++)
            {
                runtimeDeck.Add(entry);
            }

        }
    }

    // 2. 현재 덱 가져오기
    public List<CardDataEntry> GetCurrentDeck()
    {
        return runtimeDeck;
    }

    // 3. 카드 추가
    public void AddCard(CardName cardName)
    {
        CardDataEntry newCard = cardDatabase.GetCardData(cardName);

        if (newCard != null)
        {
            runtimeDeck.Add(newCard);
            Debug.Log($"[Deck] {newCard.koreanName} 추가됨! (현재 {runtimeDeck.Count}장)");
        }
    }

    // 4. 카드 제거
    public void RemoveCard(CardDataEntry card)
    {
        if (runtimeDeck.Contains(card))
        {
            runtimeDeck.Remove(card);
            Debug.Log($"[Deck] {card.koreanName} 제거됨. (현재 {runtimeDeck.Count}장)");
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private CardData cardData; 

   private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // [안전 장치] 만약 데이터가 연결 안 됐으면 에러 띄우기
        if (cardData == null)
        {
            Debug.LogError("🚨 [CardManager] 치명적 오류: CardData가 연결되지 않았습니다! 인스펙터를 확인하세요.");
        }
    }

    public Card CreateCard(CardName cardName, bool isEnchanted, Vector3 spawnPos, Transform parent)
    {
        CardDataEntry dataEntry = null;

        // 1. 강화 여부에 따라 가져올 데이터 결정
        if (isEnchanted)
        {
            dataEntry = cardData.GetEnchantedCardData(cardName);
            
            // 강화 데이터가 없으면 로그 한 번 찍어보기 (디버깅용)
            if (dataEntry == null)
            {
                Debug.LogWarning($"[CardManager] {cardName}의 강화 데이터(+)가 없습니다. 원본 데이터를 찾습니다.");
            }
        }

        // 2. 강화 데이터가 없거나(강화 안 된 경우 포함), 일반 생성인 경우 원본 데이터 검색
        if (dataEntry == null)
        {
            dataEntry = cardData.GetCardData(cardName);
        }

        // ▼▼▼ [에러 해결 핵심] 데이터가 없으면 여기서 멈춰야 합니다! ▼▼▼
        if (dataEntry == null)
        {
            Debug.LogError($"🚨 [CardManager] 치명적 오류: '{cardName}'에 해당하는 카드 데이터를 찾을 수 없습니다! CardData(ScriptableObject)에 등록되어 있는지 확인하세요.");
            return null;
        }

        if (dataEntry.cardPrefab == null)
        {
            Debug.LogError($"🚨 [CardManager] '{cardName}'의 데이터는 찾았으나, 'Card Prefab'이 비어있습니다(None). 인스펙터를 확인하세요.");
            return null;
        }
        // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

        // 3. 프리팹 생성 
        Card cardObj = Instantiate(dataEntry.cardPrefab, spawnPos, Quaternion.identity, parent);
        
        cardObj.Setup(dataEntry);

        return cardObj;
    }
    
    public CardDataEntry GetCardData(CardName name) => cardData.GetCardData(name);
    public CardDataEntry GetEnchantedCardData(CardName name) => cardData.GetEnchantedCardData(name);

    public List<CardDataEntry> GetCardsByElement(Element element)
    {
        // CardData에 있는 기능을 호출해서 그대로 반환
        return cardData.GetCardsByElement(element);
    }

    public List<CardName> GetAllCardNames()
    {
        // CardData에게 리스트 요청
        return cardData.GetAllCardNames();
    }

    public List<CardDataEntry> GetAllCardData()
    {   
        //  CardData에게 데이터 리스트 요청
        return cardData.GetAllCardData();
    }
}
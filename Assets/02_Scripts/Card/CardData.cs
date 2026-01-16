using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData", order = 0)]
public class CardData : ScriptableObject
{
    [SerializeField] private string spriteBasePath = "03_Images/Cards";
    [SerializeField] private string prefabBasePath = "05_Prefabs/Cards";
    [SerializeField] private bool useElementFolder = true;
    [SerializeField] private List<CardDataEntry> cards = new();

    // Element별로 List, CardName으로 직접 조회
    private Dictionary<Element, List<CardDataEntry>> elementMap;
    private Dictionary<CardName, CardDataEntry> cardNameMap;

    private void OnEnable()
    {
        BuildCardDataMap();
    }

    private void BuildCardDataMap()
    {
        elementMap = new Dictionary<Element, List<CardDataEntry>>();
        cardNameMap = new Dictionary<CardName, CardDataEntry>();

        foreach (CardDataEntry entry in cards)
        {
            // Element별 List
            if (!elementMap.ContainsKey(entry.element))
            {
                elementMap[entry.element] = new List<CardDataEntry>();
            }
            elementMap[entry.element].Add(entry);

            // CardName으로 직접 조회
            cardNameMap[entry.cardName] = entry;
        }
    }

    public Sprite GetCardSprite(CardName cardName)
    {
        CardDataEntry entry = GetCardData(cardName);
        return entry?.cardSprite;
    }

    public Card GetCardPrefab(CardName cardName)
    {
        CardDataEntry entry = GetCardData(cardName);
        return entry?.cardPrefab;
    }

    public CardDataEntry GetCardData(CardName cardName)
    {
        if (cardNameMap == null)
            BuildCardDataMap();

        return cardNameMap[cardName]; // 없으면 자동으로 KeyNotFoundException 발생
    }

    public List<CardName> GetAllCardNames()
    {
        // 전체카드 이름만 다 뽑아서 리스트로 만들어서 StoreUI에 전달
        return cards.Select(entry => entry.cardName).ToList();
    }


    public List<CardDataEntry> GetCardsByElement(Element element)
    {
        if (elementMap == null)
            BuildCardDataMap();

        return elementMap[element]; // 없으면 자동으로 KeyNotFoundException 발생
    }

    public IEnumerable<CardDataEntry> GetAllCards()
    {
        return cards;
    }

#if UNITY_EDITOR
    [ContextMenu("Import From JSON")]
    public void ImportFromJson()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Card Data JSON", Application.dataPath, "json");
        
        if (string.IsNullOrEmpty(jsonPath))
            return;

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(jsonText);
            
            cards.Clear();

            foreach (JsonCardData jsonCard in wrapper.cards)
            {
                // CardName 파싱
                if (!Enum.TryParse(jsonCard.cardName, true, out CardName parsedCardName))
                {
                    Debug.LogWarning($"Invalid CardName: {jsonCard.cardName}");
                    continue;
                }

                // Element 파싱
                if (!Enum.TryParse(jsonCard.element, true, out Element parsedElement))
                {
                    Debug.LogWarning($"Invalid Element: {jsonCard.element}");
                    continue;
                }

                //string {value1}, {value2} 를 숫자로 변환
                string processedDescription = jsonCard.description;

                // {value1}이 있다면 jsonCard.value1 값으로 교체
                if (!string.IsNullOrEmpty(processedDescription))
                {
                    processedDescription = processedDescription.Replace("{value1}", jsonCard.value1.ToString());
                    processedDescription = processedDescription.Replace("{value2}", jsonCard.value2.ToString());
                }

                CardDataEntry entry = new()
                {
                    cardName = parsedCardName,
                    koreanName = jsonCard.koreanName ?? string.Empty,
                    element = parsedElement,
                    isSpecial = jsonCard.isSpecial,
                    description = processedDescription,
                    price = jsonCard.price,
                    isExtinct = jsonCard.isExtinct, 
                    cost = jsonCard.cost,           
                    //cardCost = jsonCard.cardCost   
                };

                // Sprite 로드
                string spritePath = GetAssetPath(spriteBasePath, jsonCard.cardName, parsedElement, ".png");
                entry.cardSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (entry.cardSprite == null)
                    Debug.LogWarning($"Sprite not found: {spritePath}");

                // Prefab 로드
                string prefabPath = GetAssetPath(prefabBasePath, jsonCard.cardName, parsedElement, ".prefab");
                entry.cardPrefab = AssetDatabase.LoadAssetAtPath<Card>(prefabPath);
                if (entry.cardPrefab == null)
                    Debug.LogWarning($"Prefab not found: {prefabPath}");
                if (entry.cardPrefab != null)
                {
                    SerializedObject so = new SerializedObject(entry.cardPrefab);
                    
                    SerializedProperty costProp = so.FindProperty("initCost"); 
                    
                    if (costProp != null)
                    {
                        costProp.intValue = jsonCard.cost; // JSON 값으로 덮어쓰기
                        so.ApplyModifiedProperties();      // 변경사항 적용
                    }
                }
                cards.Add(entry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            // Dictionary 재생성
            BuildCardDataMap();

            Debug.Log($"Successfully imported {cards.Count} cards from JSON");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import JSON: {e.Message}");
        }
    }

    private string GetAssetPath(string basePath, string fileName, Element element, string extension)
    {
        string relativePath = useElementFolder
            ? Path.Combine(basePath, element.ToString(), fileName + extension)
            : Path.Combine(basePath, fileName + extension);
        
        string fullPath = Path.Combine("Assets", relativePath);
        Debug.Log($"Looking for asset: {fullPath}");
        return fullPath;
    }
#endif
}

[Serializable]
public class CardDataEntry
{
    public CardName cardName;
    public string koreanName;
    public Sprite cardSprite;
    public Element element;
    public bool isSpecial;
    public bool isExtinct;
    public Card cardPrefab;
    public int price;
    public int cost;
    [TextArea] public string description;
    //[TextArea] public string cardCost;

}

#if UNITY_EDITOR
[Serializable]
public class JsonWrapper
{
    public List<JsonCardData> cards;
}

[Serializable]
public class JsonCardData
{
    public string cardName;
    public string koreanName;
    public string element;
    public bool isSpecial;
    public bool isExtinct;
    public string description;   
    public int price; 
    public int value1; 
    public int value2;
    public int cost;
    public string cardCost;
}
#endif

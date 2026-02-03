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
        
        [Header("Data Lists")]
        [SerializeField] private List<CardDataEntry> cards = new();            // 일반 카드 리스트
        [SerializeField] private List<CardDataEntry> enchantedCards = new();   // 강화 카드 리스트

        // Element별로 List, CardName으로 직접 조회
        private Dictionary<Element, List<CardDataEntry>> elementMap;
        private Dictionary<CardName, CardDataEntry> cardNameMap;
        private Dictionary<CardName, CardDataEntry> enchantedCardMap;

        private void OnEnable()
        {
            BuildCardDataMap();
        }

        private void BuildCardDataMap()
        {
            elementMap = new Dictionary<Element, List<CardDataEntry>>();
            cardNameMap = new Dictionary<CardName, CardDataEntry>();
            enchantedCardMap = new Dictionary<CardName, CardDataEntry>();

            // 일반 카드 매핑
            foreach (CardDataEntry entry in cards)
            {
                if (!elementMap.ContainsKey(entry.element))
                {
                    elementMap[entry.element] = new List<CardDataEntry>();
                }
                elementMap[entry.element].Add(entry);
                cardNameMap[entry.cardName] = entry;
            }

            // 강화 카드 매핑
            foreach (CardDataEntry entry in enchantedCards)
            {
                if (!enchantedCardMap.ContainsKey(entry.cardName))
                {
                    enchantedCardMap[entry.cardName] = entry;
                }
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
            // 일반 카드에서 찾기
            if (cardNameMap.TryGetValue(cardName, out var entry))
                return entry;
            
            return null; 
        }

        // 강화카드 데이터 가져오기
        public CardDataEntry GetEnchantedCardData(CardName cardName)
        {
            return enchantedCardMap.TryGetValue(cardName, out var entry) ? entry : null;
        }

    public List<CardName> GetAllCardNames()
        {
            List<CardName> names = new List<CardName>();
            
            foreach (var card in cards) 
            {
                names.Add(card.cardName);
            }        
            return names;
        }
        public List<CardDataEntry> GetAllCardData()
        {
            // 원본 리스트 보호를 위해 새 리스트로 만들어서 반환
            return new List<CardDataEntry>(cards);
        }

        public List<CardDataEntry> GetCardsByElement(Element element)
        {
            return elementMap.ContainsKey(element) ? elementMap[element] : new List<CardDataEntry>();
        }

        public IEnumerable<CardDataEntry> GetAllCards()
        {
            return cards;
        }

    #if UNITY_EDITOR

        // 1. 일반 카드 임포트 (Import Standard Cards)
        [ContextMenu("Import Standard Cards")]
        public void ImportStandardFromJson()
        {
            string jsonPath = EditorUtility.OpenFilePanel("Select Standard Card Data JSON", Application.dataPath, "json");
            if (string.IsNullOrEmpty(jsonPath)) return;

            try
            {
                string jsonText = File.ReadAllText(jsonPath);
                
                // JSON이 배열인 경우 처리
                if (jsonText.TrimStart().StartsWith("["))
                {
                    // 배열을 객체 래퍼로 감싸기
                    jsonText = "{\"cards\":" + jsonText + "}";
                }
                
                JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(jsonText);

                cards.Clear(); // 일반 카드 리스트 초기화

                foreach (JsonCardData jsonCard in wrapper.cards)
                {
                    if (!Enum.TryParse(jsonCard.cardName, true, out CardName parsedCardName)) continue;
                    if (!Enum.TryParse(jsonCard.element, true, out Element parsedElement)) continue;

                    string processedDescription = jsonCard.description;
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
                        isExtinct = jsonCard.isExtinct,
                        description = processedDescription,
                        price = jsonCard.price,
                        cost = jsonCard.cost,
                        isMachineArmActive = jsonCard.machineArm,
                        effectDelay = jsonCard.effectDelay,
                    };

                    // EffectType 할당 (쉼표로 구분된 여러 타입 지원)
                    entry.effectTypes = ParseEffectTypes(jsonCard.effectType.ToString());

                    // Sprite 로드
                    string spritePath = GetAssetPath(spriteBasePath, jsonCard.cardName, parsedElement, ".png");
                    entry.cardSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                    
                    // Prefab 로드
                    string prefabPath = GetAssetPath(prefabBasePath, jsonCard.cardName, parsedElement, ".prefab");
                    entry.cardPrefab = AssetDatabase.LoadAssetAtPath<Card>(prefabPath);

                    // 프리팹 내부 코스트 업데이트
                    SerializedObject so = new SerializedObject(entry.cardPrefab);
                    SerializedProperty costProp = so.FindProperty("initCost");
                    costProp.intValue = jsonCard.cost;
                    so.ApplyModifiedProperties();


                    cards.Add(entry);
                }

                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                BuildCardDataMap(); // 딕셔너리 갱신

                Debug.Log($"Successfully imported {cards.Count} STANDARD cards.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to import Standard JSON: {e.Message}");
            }
        }

        // 2. 강화 카드 임포트 (Import Enchanted Cards)
        [ContextMenu("Import Enchanted Cards")]
        public void ImportEnchantedFromJson()
        {
            string jsonPath = EditorUtility.OpenFilePanel("Select Enchanted Card Data JSON", Application.dataPath, "json");
            if (string.IsNullOrEmpty(jsonPath)) return;

            try
            {
                string jsonText = File.ReadAllText(jsonPath);
                
                // JSON이 배열인 경우 처리
                if (jsonText.TrimStart().StartsWith("["))
                {
                    // 배열을 객체 래퍼로 감싸기
                    jsonText = "{\"enchantcards\":" + jsonText + "}";
                }
                
                JsonEnchantWrapper wrapper = JsonUtility.FromJson<JsonEnchantWrapper>(jsonText);
                
                enchantedCards.Clear(); // 강화 카드 리스트 초기화
                BuildCardDataMap();     // 일반 카드 맵 빌드

                foreach (JsonCardData jsonCard in wrapper.enchantcards)
                {
                    if (!Enum.TryParse(jsonCard.cardName, true, out CardName parsedCardName)) continue;
                    if (!Enum.TryParse(jsonCard.element, true, out Element parsedElement)) continue;

                        string processedDescription = jsonCard.description;
                        processedDescription = processedDescription.Replace("{value1}", jsonCard.value1.ToString());
                        processedDescription = processedDescription.Replace("{value2}", jsonCard.value2.ToString());

                        CardDataEntry entry = new()
                        {
                            cardName = parsedCardName,
                            koreanName = jsonCard.koreanName ?? string.Empty,
                            element = parsedElement,
                            isSpecial = jsonCard.isSpecial,
                            isExtinct = jsonCard.isExtinct,
                            description = processedDescription,
                            price = jsonCard.price,
                            cost = jsonCard.cost,
                            isMachineArmActive = jsonCard.machineArm,
                            effectDelay = jsonCard.effectDelay, 
                        };

                        // EffectType 할당 (쉼표로 구분된 여러 타입 지원)
                        entry.effectTypes = ParseEffectTypes(jsonCard.effectType.ToString());

                        // 원본 리소스 복사
                        if (cardNameMap.ContainsKey(parsedCardName))
                        {
                            CardDataEntry original = cardNameMap[parsedCardName];
                            entry.cardSprite = original.cardSprite;
                            entry.cardPrefab = original.cardPrefab;
                        }
                        else
                        {
                            Debug.LogWarning($"Original card not found: {parsedCardName} (Make sure Standard Cards are imported first!)");
                        }

                        enchantedCards.Add(entry);
                    }

                    EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                BuildCardDataMap();

                Debug.Log($"Successfully imported {enchantedCards.Count} ENCHANTED cards.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to import Enchanted JSON: {e.Message}");
            }
        }

        private string GetAssetPath(string basePath, string fileName, Element element, string extension)
        {
            string relativePath;

            if (useElementFolder)
            {
                relativePath = Path.Combine(basePath, element.ToString(), fileName + extension);
            }
            else
            {
                relativePath = Path.Combine(basePath, fileName + extension);
            }
            
            return Path.Combine("Assets", relativePath).Replace("\\", "/");
        }

        private List<EffectType> ParseEffectTypes(string effectTypeString)
        {
            List<EffectType> result = new();
            if (string.IsNullOrEmpty(effectTypeString)) return result;

            // 쉼표로 구분된 숫자들을 파싱 (예: "21" 또는 "21,22")
            string[] typeStrings = effectTypeString.Split(',');
            foreach (string typeStr in typeStrings)
            {
                if (int.TryParse(typeStr.Trim(), out int typeValue))
                {
                    if (System.Enum.IsDefined(typeof(EffectType), typeValue))
                    {
                        result.Add((EffectType)typeValue);
                    }
                }
            }

            return result;
        }
    #endif
    }

    // 기본 카드 JSON용
    #if UNITY_EDITOR
    [Serializable]
    public class JsonWrapper
    {
        public List<JsonCardData> cards;
    }

    // 강화 카드 JSON용
    [Serializable]
    public class JsonEnchantWrapper
    {
        public List<JsonCardData> enchantcards;
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
        public bool machineArm;
        public string effectType;              // 이펙트 타입 번호 ("21" 또는 "1,8" 형식)
        public float effectDelay;
    }
    #endif

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
        
        // 이펙트 관련
        public List<EffectType> effectTypes = new();  // 여러 이펙트 타입 지원
        public bool isMachineArmActive;            // 기계팔 이펙트 사용 여부
        public float effectDelay;                  // 이펙트 재생 딜레이
    }
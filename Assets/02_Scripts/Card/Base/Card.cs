using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System;

[Flags]
public enum CardType
{
    Attack,  // 공격
    Defense, // 방어
    BuffCard,    // 버프
    Debuff   // 디버프
}

[RequireComponent(typeof(SpriteRenderer))]

// Prefab
abstract public class Card : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] protected Element element;
    [SerializeField] protected int initCost;            // 코스트
    [SerializeField] protected bool isExtinct; // 소멸 여부 
    [SerializeField] protected bool isSpecial; // 특수 카드 여부

    [Header("Effect Settings")]
    [SerializeField] protected List<EffectType> effectTypes = new();    // 여러 이펙트 타입 번호 지원
    //[SerializeField] protected Transform effectSpawnPosition; // 이펙트 생성 위치

    [Header("UI Reference")]
    [SerializeField] TMP_Text cardTitle;
    [SerializeField] TMP_Text cardDesc;
    [SerializeField] TMP_Text cardCost;

    [Header("Color Settings")]
    [SerializeField] private Color activeColor = new(255, 255, 255);
    [SerializeField] private Color unActiveColor = new(125, 125, 125);


    // 이동 코루틴
    private Coroutine moveCoroutine;
    private Vector3 localScale;
    private SpriteRenderer sprite;
    private bool isDiscardSelect = false;

    protected int curCost;
    protected CardDataEntry cardData;

    // Property
    public int Cost => curCost;
    public Vector3 OriginPos { get; set; } = new();
    public CardDataEntry Data => cardData;
    public Element Element => element;

    public virtual bool IsExtinct => isExtinct;
    public virtual bool IsSpecial => isSpecial;
    public bool IsEnchanted { get; private set; }

    public List<EffectType> EffectTypes => effectTypes;
    
    /// <summary>
    /// 이펙트들을 생성하고 재생합니다. (여러 이펙트 동시 지원)
    /// </summary>
    protected IEnumerator PlayEffect(Target target)
    {
        if (Debug.isDebugBuild)
        if (effectTypes == null || effectTypes.Count == 0)
            yield break;

        Enemy targetEnemy = target as Enemy;
        if (targetEnemy == null)
            yield break;

        float playDelay = cardData?.effectDelay ?? 0f;

        // effectType 순서대로 실행 (딜레이가 0이면 사실상 동시에 시작)
        for (int effectIndex = 0; effectIndex < effectTypes.Count; effectIndex++)
        {
            if (playDelay > 0f && effectIndex > 0)
                yield return new WaitForSeconds(playDelay);

            EffectType effectType = effectTypes[effectIndex];
            EffectDataEntry effectData = EffectManager.Instance.GetEffectData(effectType);
            if (effectData == null)
                continue;

            int[,] patternArray = effectData.effectPattern;
            int colCount = patternArray.GetLength(1);
            
            // 각 column마다 EffectControl 생성 (모두 동시에 시작)
            for (int colIdx = 0; colIdx < colCount; colIdx++)
            {
                EffectControl effectControl = Instantiate(effectData.effectPrefab);
                effectControl.Play(patternArray, effectData.effectDuration, targetEnemy, colIdx);
            }
            
            // 이펙트 재생 시간만큼 대기
            float totalEffectTime = 0f;
            if (effectData.effectDuration != null && effectData.effectDuration.Length > 0)
            {
                foreach (float duration in effectData.effectDuration)
                    totalEffectTime += duration;
            }
            
            if (totalEffectTime > 0f)
                yield return new WaitForSeconds(totalEffectTime);
        }
    }
    public bool IsDiscardSelect
    {
        get => isDiscardSelect;
        set
        {
            isDiscardSelect = value;
            sprite.color = value ? unActiveColor : activeColor;
        }
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        localScale = transform.localScale;
        curCost = initCost;
    }

    protected virtual void OnEnable()
    {
        curCost = initCost;
        isDiscardSelect = false;
        transform.localScale = localScale;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    protected virtual void Start()
    {   
        Player player = BattleManager.Instance.Player;
        player.OnTurnStart.AddListener(HandleTurnStart);
    }

    public void Init(CardDataEntry cardDataEntry)
    {
        cardData = cardDataEntry;
        SetupDate(cardData);

        IsEnchanted = false;
    }

    public void Enhance()
    {
        cardData = CardManager.Instance.GetEnchantedCardData(Name);
        SetupDate(cardData);

        IsEnchanted = true;
    }

    private void SetupDate(CardDataEntry cardDataEntry)
    {
        cardTitle.text = cardDataEntry.koreanName;
        cardDesc.text = cardDataEntry.description;
        cardCost.text = cardDataEntry.cost.ToString();

        element = cardDataEntry.element;
        isSpecial = cardDataEntry.isSpecial;
        isExtinct = cardDataEntry.isExtinct;

        initCost = cardDataEntry.cost;
        curCost = initCost;

        // 이펙트 타입 로드 (여러 타입 지원)
        effectTypes = new List<EffectType>(cardDataEntry.effectTypes);
        
    }


    // Unity 마우스 이벤트
    private void OnMouseEnter()
    {
        if (moveCoroutine != null || isDiscardSelect) return;
        BattleManager.Instance.Player.OnCardEnter(this);
    }

    private void OnMouseExit()
    {
        if (moveCoroutine != null || isDiscardSelect) return;
        BattleManager.Instance.Player.OnCardExit(this);
    }

    private void OnMouseDown()
    {
        if (moveCoroutine != null || isDiscardSelect) return;
        Debug.Log($"[Card.OnMouseDown] Clicked card: {Name}");
        BattleManager.Instance.Player.OnCardClick(this);
    }

    public void Select()
    {
        transform.localScale = localScale * 1.2f;

        Vector3 newPos = transform.localPosition;
        newPos.z = -3.0f;  // 최상단으로
        newPos.y = OriginPos.y + 0.5f;
        transform.localPosition = newPos;
    }

    public void UnSelect()
    {
        transform.localScale = localScale;
        transform.localPosition = OriginPos;  // 원래 위치로
    }

    // 목표 위치로 부드럽게 이동하는 함수
    public void MoveTo(Vector3 targetLocalPos, UnityAction onComplete = null)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        OriginPos = targetLocalPos;
        moveCoroutine = StartCoroutine(MoveRoutine(targetLocalPos, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, UnityAction onComplete, float duration = 0.5f)
    {
        float time = 0;
        Vector3 startPos = transform.localPosition;

        while (time < duration)
        {
            float t = time / duration;
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);

            transform.localPosition = Vector3.Lerp(startPos, targetPos, smoothT);
            time += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = targetPos;        //도착 후 정확한 위치로 고정
        moveCoroutine = null;

        // 도착 후 실행할 행동(파괴 등)이 있다면 실행
        onComplete?.Invoke();
    }

    private void HandleTurnStart()
    {
        Player player = BattleManager.Instance.Player;

        if (player.Status.Dizzy.IsActive)
            SetCost(curCost + 1);
        else
            SetCost(initCost);
    }

    // 임시 코스트 조정함수
    public void SetCost(int newCost)
    {
        curCost = newCost;
        cardCost.text = curCost.ToString();

        if (newCost < initCost) cardCost.color = Color.yellow;
        else cardCost.color = Color.white;
    }

    abstract public CardName Name { get; }
    abstract public CardType Type { get; }

    public int Use(Player user, Target target)
    {
        user.StartCoroutine(InternalUseRoutine(user, target));
        return Cost;
    }

    private IEnumerator InternalUseRoutine(Player user, Target target)
    {
        BattleManager.Instance.Pause();

        yield return UseRoutine(user, target);

        BattleManager.Instance.Restart();
    }

    abstract protected IEnumerator UseRoutine(Player user, Target target);
}
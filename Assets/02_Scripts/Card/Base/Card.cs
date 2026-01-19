using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public enum CardType
{
    Attack,  // 공격
    Defense, // 방어
    BuffCard,    // 버프
    Debuff   // 디버프
}

// Prefab
abstract public class Card : MonoBehaviour
{
    //카드 기본 정보
    [SerializeField] protected Element element;
    [SerializeField] protected int initCost;            // 코스트
    [SerializeField] protected bool isExtinct; // 소멸 여부 
    [SerializeField] protected bool isSpecial; // 특수 카드 여부
    [SerializeField] TMP_Text cardTitle;
    [SerializeField] TMP_Text cardDesc;
    [SerializeField] TMP_Text cardCost;
    
    // 이동 코루틴
    private Coroutine moveCoroutine;
    private Vector3 localScale;
    private ICardEventHandler handler;
    protected int curCost;

    // 카드 정보 관련
    //public bool IsSpecial { get; }      // 특수 카드 여부 
    //public bool IsException { get; }     //제외 카드 여부

    // Property
    public int Cost => curCost;
    public Vector3 OriginPos { get; set; } = new();
    public bool IsEntered { get; set; } = false;
    public virtual bool IsExtinct => isExtinct; 
    public virtual bool IsSpecial => isSpecial;
    public bool IsEnchanted { get; private set; }

    private void Awake()
    {
        localScale = transform.localScale;
        curCost = initCost;
    }

    protected virtual void Start()
    {
        Player player = BattleManager.Instance.Player;
        handler = player;
        player.OnTurnStart.AddListener(HandleTurnStart);
    }

    // 외부에서 데이터를 주입해주는 초기화 함수
    public void Setup(CardDataEntry data)
    {
        if (data == null) return;

        // 1. 텍스트 정보 동기화
        if (cardTitle) cardTitle.text = data.koreanName; // 강화된 이름
        if (cardDesc) cardDesc.text = data.description;
        if (cardCost) cardCost.text = data.cost.ToString();

        // 2. 스탯 정보 덮어씌우기
        this.element = data.element;
        this.isSpecial = data.isSpecial;
        this.isExtinct = data.isExtinct;     
        
        this.initCost = data.cost;
        this.curCost = data.cost;

        // 이름에 "+"가 포함되어 있거나, 데이터 자체가 강화 데이터라면 true로 설정
        this.IsEnchanted = data.koreanName.EndsWith("+");        
        
    }

    // Unity 마우스 이벤트
    private void OnMouseEnter()
    {
        if (moveCoroutine != null) return;
        handler?.OnCardEnter(this);
    }

    private void OnMouseExit()
    {
        if (moveCoroutine != null) return;
        handler?.OnCardExit(this);
    }

    private void OnMouseDown()
    {
        if (moveCoroutine != null) return;
        handler?.OnCardClick(this);
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
            transform.localPosition = Vector3.Lerp(startPos, targetPos, time / duration);       //Lerp 이용해서 부드럽게 이동
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
        // 만약 UI에 코스트가 표시된다면 갱신하는 코드가 여기에 들어가야 함        
    }    

    abstract public CardName Name { get; }
    abstract public CardType Type { get; }
    abstract public int Use(Player user, Target target);
}
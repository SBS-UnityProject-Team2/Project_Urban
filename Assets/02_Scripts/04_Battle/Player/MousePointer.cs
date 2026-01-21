using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MousePointer : MonoBehaviour
{
    private readonly string LEFT_CLICK = "LeftClick";
    private readonly string RIGHT_CLICK = "RightClick";
    private readonly string MOVE = "move";
    private readonly float MAX_DISTANCE = 100.0f;

    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private LayerMask enemyLayer;

    // Component
    private Camera mainCam;
    private PlayerInput playerInput;
    private Ray currentMouseRay;

    // Card 
    private Card selectedCard;
    private Card hoveredCard;

    // Enemy
    private Enemy hoveredEnemy;

    private readonly RaycastHit2D[] raycastHitBuffer = new RaycastHit2D[1];

    // Card Event 
    public UnityEvent<Card> OnCardSelect { get; set; } = new();
    public UnityEvent<Card> OnCardUnSelect { get; set; } = new();
    public UnityEvent<Card> OnCardEnter { get; set; } = new();
    public UnityEvent<Card> OnCardExit { get; set; } = new();

    // Enemy Event
    public UnityEvent<Enemy> OnEnemySelect { get; set; } = new();
    public UnityEvent<Enemy> OnEnemyEnter { get; set; } = new();
    public UnityEvent<Enemy> OnEnemyExit { get; set; } = new();

    public Card SelectedCard => selectedCard;

    public void ClearSelection()
    {
        selectedCard = null;
        ClearHoveredCard();
        ClearHoveredEnemy();
    }

    private void ClearHoveredCard()
    {
        if (hoveredCard != null)
        {
            OnCardExit?.Invoke(hoveredCard);
            hoveredCard = null;
        }
    }

    private void ClearHoveredEnemy()
    {
        if (hoveredEnemy != null)
        {
            OnEnemyExit?.Invoke(hoveredEnemy);
            hoveredEnemy = null;
        }
    }

    private void Awake()
    {
        mainCam = Camera.main;
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions[LEFT_CLICK].started += HandleLeftClick;
        playerInput.actions[RIGHT_CLICK].started += HandleRightClick;
        playerInput.actions[MOVE].performed += HandleMove;
    }

    private void Update()
    {
        if (!BattleManager.Instance.IsPlayerTurn)
        {
            ClearHoveredCard();
            ClearHoveredEnemy();
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        currentMouseRay = mainCam.ScreenPointToRay(mousePosition);
    }

    private void HandleLeftClick(InputAction.CallbackContext context)
    {
        if (!BattleManager.Instance.IsPlayerTurn) return;

        HandleCardClick();

        if (selectedCard != null)
            HandleEnemyClick();
    }

    private void HandleRightClick(InputAction.CallbackContext context)
    {
        if (!BattleManager.Instance.IsPlayerTurn) return;

        OnCardUnSelect?.Invoke(selectedCard);
        ClearSelection();
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        if (!BattleManager.Instance.IsPlayerTurn) return;

        HandleCardMove();

        if (selectedCard != null)
            HandleEnemyMove();
    }


    private void HandleCardClick()
    {
        if (RayCast(cardLayer))
        {
            selectedCard = raycastHitBuffer[0].collider.GetComponent<Card>();
            OnCardSelect?.Invoke(selectedCard);
        }
    }

    private void HandleEnemyClick()
    {
        if (RayCast(enemyLayer))
        {
            Enemy enemy = raycastHitBuffer[0].collider.GetComponent<Enemy>();
            OnEnemySelect?.Invoke(enemy);
        }
    }

    private void HandleCardMove()
    {
        Card curHoveredCard = null;

        if (RayCast(cardLayer))
        {
            curHoveredCard = raycastHitBuffer[0].collider.GetComponent<Card>();
        }

        if (curHoveredCard != hoveredCard)
        {
            if (hoveredCard != null)
                OnCardExit?.Invoke(hoveredCard);

            if (curHoveredCard != null)
                OnCardEnter?.Invoke(curHoveredCard);

            hoveredCard = curHoveredCard;
        }
    }

    private void HandleEnemyMove()
    {
        Enemy curHoveredEnemy = null;

        if (RayCast(enemyLayer))
        {
            curHoveredEnemy = raycastHitBuffer[0].collider.GetComponent<Enemy>();
        }

        if (curHoveredEnemy != hoveredEnemy)
        {
            if (hoveredEnemy != null)
                OnEnemyExit?.Invoke(hoveredEnemy);

            if (curHoveredEnemy != null)
                OnEnemyEnter?.Invoke(curHoveredEnemy);

            hoveredEnemy = curHoveredEnemy;
        }
    }

    private bool RayCast(LayerMask layerMask)
    {
        Vector2 origin = currentMouseRay.origin;
        Vector2 direction = currentMouseRay.direction;
        bool result = Physics2D.RaycastNonAlloc(origin, direction, raycastHitBuffer, MAX_DISTANCE, layerMask) > 0;
        return result;
    } 
}
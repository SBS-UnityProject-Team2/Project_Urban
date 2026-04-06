using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class MonsterView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Image selectArrow;
    [SerializeField] private NextActionView nextActionView;
    [SerializeField] private StatusEffectView statusEffectView;
    [SerializeField] private MonsterHealthView healthView;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        selectArrow.enabled = false;
    }

    public void Init(MonsterDataEntry monsterDataEntry, ActorStatus actorStatus, MonsterAction action)
    {
        spriteRenderer.sprite = MonsterManager.Instance.GetMonsterImage(monsterDataEntry.name);
        healthView.Bind(actorStatus);
    }

    public void Select()
    {
        selectArrow.enabled = true;
    }

    public void UnSelect()
    {
        selectArrow.enabled = false;
    }
}
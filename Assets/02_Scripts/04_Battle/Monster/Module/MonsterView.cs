using UnityEngine;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
    [SerializeField] private Image selectArrow;
    [SerializeField] private NextActionView nextActionView;
    [SerializeField] private StatusEffectView statusEffectView;
    [SerializeField] private HealthView healthView;

    public void Init(ActorStatus actorStatus, MonsterAction action)
    {
        
    }
}
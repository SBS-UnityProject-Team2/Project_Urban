using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerHeathView heathView;
    [SerializeField] private PlayerCostView costView;
    [SerializeField] private PlayerIconView playerIconView;
    [SerializeField] private PlayerStatusEffectListView statusEffectView;

    public void Init(ActorStatus status)
    {
        heathView.Bind(status);
        costView.Bind(status);
        playerIconView.Bind(status);
        statusEffectView.Bind(status);
    }
}
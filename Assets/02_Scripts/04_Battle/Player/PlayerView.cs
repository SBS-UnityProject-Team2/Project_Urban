using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerHeathView heathView;

    public void Init(ActorStatus status)
    {
        heathView.Bind(status);
    }
}
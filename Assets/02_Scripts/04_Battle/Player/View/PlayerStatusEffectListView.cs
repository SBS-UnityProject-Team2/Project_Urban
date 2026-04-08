using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffectListView : MonoBehaviour
{
    [SerializeField] private PlayerStatusEffectItemView itemPrefab;
    [SerializeField] private Transform content;
    
    public void UpdateView(IEnumerable<StatusEffect> effects)
    {
        foreach (Transform item in content)
            Destroy(item.gameObject);

        foreach(StatusEffect effect in effects)
        {
            PlayerStatusEffectItemView itemView = Instantiate(itemPrefab, content);
            itemView.Init(effect);
        }
    }

    public void Bind(ActorStatus actorStatus)
    {
        actorStatus.EffectList.OnUpdate.AddListener(UpdateView);
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CardAction : MonoBehaviour
{
    private List<ActionDataEntry> actionData;
    private readonly ActionPayload payload = new();

    public void Init(int linkId)
    {
        actionData = CardManager.Instance.GetActionData(linkId);
    }
}
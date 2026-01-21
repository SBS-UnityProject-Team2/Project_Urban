using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class NextActionView : MonoBehaviour
{
    private TMP_Text nextActionText;

    private void Awake()
    {
        nextActionText = GetComponent<TMP_Text>();
    }

    public void SetNextActionText(string text)
    {
        nextActionText.text = text;
    }
}
using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ModalWindowManager))]
public class TipUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Battle.Instance.IsPause = false;
            GetComponent<ModalWindowManager>().ModalWindowOut();
        });
    }

    public void Open()
    {
        Battle.Instance.IsPause = true;
        GetComponent<ModalWindowManager>().ModalWindowIn();
    }
}
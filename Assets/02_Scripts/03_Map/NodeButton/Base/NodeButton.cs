using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
abstract public class NodeButton : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    abstract public void OnClick();
}

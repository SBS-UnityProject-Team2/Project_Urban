using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnClickHandler : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void AddClickHandler(UnityAction handleClick)
    {   
        // 버튼 컴포넌트 연결
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        // 버튼 클릭 이벤트 연결
        if (button != null)
        {
            button.onClick.RemoveAllListeners(); 
            button.onClick.AddListener(handleClick);
        }       
    }
} 
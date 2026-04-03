using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonEvent : SceneSingleton<ButtonEvent>
{
    [SerializeField] private GameObject UI_Shelter;
    [SerializeField] private GameObject UI_Map;
    [SerializeField] private GameObject UI_EnterShelter;
    [SerializeField] private GameObject Panel_ShelterPopup;
    [SerializeField] private GameObject Panel_ShelterCardEnchantPopup;
    [SerializeField] private GameObject Panel_EnchantCardPopup;
    [SerializeField] private GameObject UI_Event;

    public void EnterNodeType(NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.Monster:
                EnterNormalNode();
                break;
            case NodeType.Elite:
                EnterEliteNode();
                break;
            case NodeType.Store:
                EnterStoreNode();
                break;
            case NodeType.Shelter:
                EnterShelterNode();
                break;
            case NodeType.Event:
                EnterEventNode();
                break;
            case NodeType.Boss:
                EnterBossNode();
                break;
            default:
                Debug.LogWarning($"잘못연결함");
                break;
        }
    }

    private void EnterNormalNode()
    {
        int currentFloor = 0;

        if (MapManager.Instance != null)
            currentFloor = MapManager.Instance.GetCurrentFloor();
        else
            Debug.LogWarning("⚠️ MapManager가 없어서 층수를 0으로 가정합니다.");

        if (currentFloor == 0)       EnterBattle(true, 2, 2);
        else if (currentFloor < 8)   EnterBattle(true, 2, 3);
        else                         EnterBattle(true, 3, 4);
    }

    private void EnterEliteNode()
    {
        EnterBattle(false, 5, 5);
        if (BgmManager.Instance != null)
        {
            BgmManager.Instance.PlayEliteSound();
        }
    }

    private void EnterBossNode()
    {
        EnterBattle(false, 9, 9);
        if (BgmManager.Instance != null)
        {
            BgmManager.Instance.PlayBossSound();
        }
    }

    private void EnterBattle(bool isNormal, int minScore, int maxScore)
    {
        SceneManager.LoadScene(SceneName.Battle);
    }

    private void EnterStoreNode()
    {
        // Store 씬으로 전환하기 전에 음악 재생 (씬 전환 후는 BgmManager가 초기화 중일 수 있음)
        if (BgmManager.Instance != null)
        {
            BgmManager.Instance.PlayShopSound();
        }
        
        SceneManager.LoadScene(SceneName.Store);
    }

    // 쉼터UI
    public void OnClickShelter()
    {
        EnterShelterNode();
    }

    private void EnterShelterNode()
    {
        UI_Shelter.SetActive(true);
        UI_Map.SetActive(false);
        UI_EnterShelter.SetActive(true);

        if (BgmManager.Instance != null)
        {
            BgmManager.Instance.PlayRestSound();
        }
    }

    public void OnClickEnterShelter()
    {
        UI_EnterShelter.SetActive(false);
        Panel_ShelterPopup.SetActive(true);

        BackgroundManager.Instance.SetRestBg();
    }

    public void OnClickShelterCardEnchant()
    {
        Panel_ShelterCardEnchantPopup.SetActive(true);
    }

    public void OnClickShelterExit()
    {
        Panel_ShelterPopup.SetActive(false);
        UI_EnterShelter.SetActive(true);
        UI_Shelter.SetActive(false);
        UI_Map.SetActive(true);

        BackgroundManager.Instance.SetMapBg();
        if (BgmManager.Instance != null)
        {
            BgmManager.Instance.PlayMapSound();
        }
    }

    public void ActiveMap()
    {
        UI_Map.SetActive(true);
    }

    public void OnClickEvent()
    {
        EnterEventNode();
    }

    private void EnterEventNode()
    {
        UI_Event.SetActive(true);
        UI_Map.SetActive(false);
    }

    public void OnClickEventExit()
    {
        UI_Event.SetActive(false);
        UI_Map.SetActive(true);
    }

    public void OnClickEnchantPopupExit()
    {
        Panel_EnchantCardPopup.SetActive(false);
    }
}
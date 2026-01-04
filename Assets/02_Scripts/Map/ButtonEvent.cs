using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class ButtonEvent : SceneSingleton<ButtonEvent>
{

    [SerializeField] private GameObject UI_Store;
    [SerializeField] private GameObject UI_Shelter;
    [SerializeField] private GameObject UI_Map;
    [SerializeField] private GameObject UI_EnterShelter;
    [SerializeField] private GameObject Panel_ShelterPopup;
    [SerializeField] private GameObject Panel_ShelterCardEnchantPopup;


    [SerializeField] private GameObject UI_Event;

    // 전투 씬 이동
    public void OnClickNormal()
    {
        int currentFloor = MapManager.Instance.GetCurrentFloor();
        GameManager.Instance.IsNormal = true;

        if (currentFloor == 0)
            GameManager.Instance.SetEnemyScore(2, 2);
        else if (currentFloor < 8)
            GameManager.Instance.SetEnemyScore(2, 3);
        else
            GameManager.Instance.SetEnemyScore(3, 4);

        SceneManager.LoadScene(Scene.Battle);
    }

    public void OnClickElite()
    {
        GameManager.Instance.SetEnemyScore(5, 5);
        GameManager.Instance.IsNormal = false;
        
        SceneManager.LoadScene(Scene.Battle);
    }

    public void OnClickBoss()
    {
        GameManager.Instance.SetEnemyScore(9, 9);
        GameManager.Instance.IsNormal = false;

        SceneManager.LoadScene(Scene.Battle);
    }

    // 상점UI
    public void OnClickStore()
    {        
        UI_Store.SetActive(true);
        UI_Map.SetActive(false);
    }

    public void CloseStoreUI()
    {
        UI_Store.SetActive(false);
        UI_Map.SetActive(true);   
    }

    public void OnClickStoreExit()
    {
        UI_Store.SetActive(false);
        UI_Map.SetActive(true);
    }

    // 쉼터UI
    public void OnClickShelter()
    {
       UI_Shelter.SetActive(true);
       UI_Map.SetActive(false);
       UI_EnterShelter.SetActive(true);
    }

    public void OnClickEnterShelter()
    {
        UI_EnterShelter.SetActive(false);
        Panel_ShelterPopup.SetActive(true);        
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
    }

    public void ActiveMap()
    {
        UI_Map.SetActive(true);
    }

    public void OnClickEvent()
    {
        UI_Event.SetActive(true);
        UI_Map.SetActive(false);
    }

    public void OnClickEventExit()
    {
        UI_Event.SetActive(false);
        UI_Map.SetActive(true);
    }
}
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
        SceneManager.LoadScene(Scene.Battle);
    }

    public void OnClickElite()
    {
        SceneManager.LoadScene(Scene.Battle);
    }

    public void OnClickBoss()
    {
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
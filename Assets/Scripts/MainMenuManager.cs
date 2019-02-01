using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public MenuState currentState = MenuState.mainMenu;

    public SettingsMenuManager SMM;
    public WeaponInventoryManager WIM;

    [Header ("Canvas Group")]
    public CanvasGroup ConfirmQuitCG;

    public static MainMenuManager currentInstance;


    public enum MenuState {
        mainMenu,
        confirmQuit,
        inventory,
        settings,
        profile,
    }
    private void Awake()
    {
        currentInstance = this;
    }

    #region Button Click Handlers
    public void OnQuitClick() {
        if (currentState != MenuState.mainMenu)
            return;
        currentState = MenuState.confirmQuit;
        ConfirmQuitCG.gameObject.SetActive(true);
        
    }
    public void OnProfileClick() {
        if (currentState != MenuState.mainMenu)
            return;
    }
    public void OnSettingsClick() {
        if (currentState != MenuState.mainMenu)
            return;
        SMM.OpenMenu();
        currentState = MenuState.settings;


    }
    public void OnInventoryClick() {
        if (currentState != MenuState.mainMenu)
            return;
        currentState = MenuState.inventory;
        WIM.OpenMenu();
    }
    public void OnPlayClick() {
        if (currentState != MenuState.mainMenu)
            return;
        SceneManager.LoadScene("SampleScene");
    }
    public void OnConfirmQuitClick() {
        if (currentState != MenuState.confirmQuit)
            return;
        Application.Quit();
    }
    public void OnCancelQuit() {
        if (currentState != MenuState.confirmQuit)
            return;
        currentState = MenuState.mainMenu;
        ConfirmQuitCG.gameObject.SetActive(false);
    }
    public void OnExitSettingsClick() {
        if (currentState != MenuState.settings)
            return;
        currentState = MenuState.mainMenu;
        SMM.CloseMenu();
    }
    public void OnExitInventoryClick() {
        currentState = MenuState.mainMenu;
    }
    #endregion
}

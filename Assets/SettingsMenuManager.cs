using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour {

    [Header("Settings Menu References")]
    public Toggle damageNumbersToggle;
    public Toggle interfaceShakeToggle;
    public Toggle particlesToggle;
    public Toggle musicToggle;
    public Toggle soundToggle;
    public CanvasGroup settingsCG;


    public void OpenMenu() {
        settingsCG.gameObject.SetActive(true);
        damageNumbersToggle.isOn = GlobalGameManager.currentInstance.GetGameData().GetSettingDmgNumbers();
        interfaceShakeToggle.isOn = GlobalGameManager.currentInstance.GetGameData().GetSettingHudShake();
        particlesToggle.isOn = GlobalGameManager.currentInstance.GetGameData().GetSettingParticles();
        musicToggle.isOn = GlobalGameManager.currentInstance.GetGameData().GetSettingMusic();
        soundToggle.isOn = GlobalGameManager.currentInstance.GetGameData().GetSettingSound();
    }
    public void CloseMenu() {
        settingsCG.gameObject.SetActive(false);
    }
    #region Settings Menu Click Handlers
    public void OnDmgNumbersToggle()
    {
        GlobalGameManager.currentInstance.GetGameData().SetSettingDmgNumbers(damageNumbersToggle.isOn);
    }
    public void OnHudShakeToggle()
    {
        GlobalGameManager.currentInstance.GetGameData().SetSettingHudShake(interfaceShakeToggle.isOn);
    }
    public void OnParticlesToggle()
    {
        GlobalGameManager.currentInstance.GetGameData().SetSettingParticles(particlesToggle.isOn);
    }
    public void OnMusicToggle()
    {
        GlobalGameManager.currentInstance.GetGameData().SetSettingMusic(musicToggle.isOn);
    }
    public void OnSoundToggle()
    {
        GlobalGameManager.currentInstance.GetGameData().SetSettingSound(soundToggle.isOn);
    }
    #endregion
}

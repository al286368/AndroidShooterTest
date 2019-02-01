using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData {

    private ShipData playerSelectedShip;
    private WeaponData playerSelectedWeapon;

    private bool settings_dmgNumbersEnabled;
    private bool settings_hudShakeEnabled;
    private bool settings_soundEnabled;
    private bool settings_musicEnabled;
    private bool settings_particlesEnabled;

    private List<WeaponData> data_weaponInventory; 

    public GameData() {
        data_weaponInventory = new List<WeaponData>();
        playerSelectedShip = new ShipData();
        for (int i = 0; i < 30; i++)
        {
            AddWeaponToInventory(new WeaponData(2.25f));
        }
        playerSelectedWeapon = data_weaponInventory[0];

        settings_dmgNumbersEnabled = true;
        settings_hudShakeEnabled = true;
        settings_soundEnabled = true;
        settings_musicEnabled = true;
        settings_particlesEnabled = true;
    }
    public void SetNewPlayerSelectedWeapon(WeaponData newWd) {
        playerSelectedWeapon = newWd;
    }
    public ShipData GetPlayerSelectedShip() {
        return playerSelectedShip;
    }
    public WeaponData GetPlayerSelectedWeapon() {
        return playerSelectedWeapon;
    }
    #region Game Settings Related Methods
    public bool GetSettingDmgNumbers() {
        return settings_dmgNumbersEnabled;
    }
    public bool GetSettingHudShake() {
        return settings_hudShakeEnabled;
    }
    public bool GetSettingSound() {
        return settings_soundEnabled;
    }
    public bool GetSettingMusic() {
        return settings_musicEnabled;
    }
    public bool GetSettingParticles() {
        return settings_particlesEnabled;
    }
    public void SetSettingDmgNumbers(bool stg) {
        settings_dmgNumbersEnabled = stg;
    }
    public void SetSettingHudShake(bool stg) {
        settings_hudShakeEnabled = stg;
    }
    public void SetSettingParticles(bool stg) {
        settings_particlesEnabled = stg;
    }
    public void SetSettingMusic(bool stg) {
        settings_musicEnabled = stg;
    }
    public void SetSettingSound(bool stg) {
        settings_soundEnabled = stg;
    }
    public int GetAmountOfWeaponsInInventory() {
        return data_weaponInventory.Count;
    }
    public WeaponData GetWeaponInInventorySlot(int index) {
        if (index >= data_weaponInventory.Count)
        {
            return null;
        }
        return data_weaponInventory[index];
    }
    public void AddWeaponToInventory(WeaponData wpn) {
        data_weaponInventory.Add(wpn);
    }
    #endregion
    #region InventorySorting
    public void SortWeaponInventoryByLevel()
    {
        data_weaponInventory.Sort(WeaponData.CompareByLevel);
    }
    public void SortWeaponInventoryByElement()
    {
        data_weaponInventory.Sort(WeaponData.CompareByElement);
    }
    public void SortWeaponInventoryByRarity()
    {
        data_weaponInventory.Sort(WeaponData.CompareByRarity);
        data_weaponInventory.Reverse();
    }
    #endregion


}

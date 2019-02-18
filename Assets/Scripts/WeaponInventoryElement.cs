﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryElement : MonoBehaviour {

    public Text levelText;
    public Text nameText;
    public Text damageText;
    public int index;

    private WeaponData wd_stored;

    [Header("ElementIcons")]

    public Image pulseBackground;
    public Image photonBackground;
    public Image cryoBackground;
    public Image electricBackground;
    public Image nuclearBackground;
    public Image plasmaBackground;
    public Image gammaBackground;
    [Header("RarityColors")]
    public Image commonBorder;
    public Image rareBorder;
    public Image exoticBorder;
    public Image hiTechBorder;
    public Image prototypeBorder;

    public void SetFor(WeaponData WD) {
        if (WD == null) {
            gameObject.SetActive(false);
            return;
        }
        wd_stored = WD;

        SetElementBackground();
        SetRarityBorder();
        SetTexts();
        gameObject.SetActive(true);
    }
    public void SetTexts() {
        levelText.text = wd_stored.GetUpgradeLevel().ToString();
        nameText.text = wd_stored.GetWeaponName();
        damageText.text = wd_stored.GetWeaponDamage().ToString() + " x" + wd_stored.GetMultishoot() + "";
    }
    public void SetElementBackground() {
        pulseBackground.gameObject.SetActive(wd_stored.GetWeaponElement() == WeaponData.DamageElement.pulse);
        photonBackground.gameObject.SetActive(wd_stored.GetWeaponElement() == WeaponData.DamageElement.photon);
        cryoBackground.gameObject.SetActive(wd_stored.GetWeaponElement() == WeaponData.DamageElement.cryo);
        electricBackground.gameObject.SetActive(wd_stored.GetWeaponElement() == WeaponData.DamageElement.electric);
        nuclearBackground.gameObject.SetActive(wd_stored.GetWeaponElement() == WeaponData.DamageElement.nuclear);
        plasmaBackground.gameObject.SetActive(wd_stored.GetWeaponElement() == WeaponData.DamageElement.plasma);
        gammaBackground.gameObject.SetActive(wd_stored.GetWeaponElement() == WeaponData.DamageElement.gamma);
    }
    public void SetRarityBorder() {
        commonBorder.gameObject.SetActive(wd_stored.GetWeaponRarity() == WeaponData.Rarity.common);
        rareBorder.gameObject.SetActive(wd_stored.GetWeaponRarity() == WeaponData.Rarity.rare);
        exoticBorder.gameObject.SetActive(wd_stored.GetWeaponRarity() == WeaponData.Rarity.exotic);
        hiTechBorder.gameObject.SetActive(wd_stored.GetWeaponRarity() == WeaponData.Rarity.hiTech);
        prototypeBorder.gameObject.SetActive(wd_stored.GetWeaponRarity() == WeaponData.Rarity.prototype);
    }
    public void OnElementClicked() {
        WeaponInventoryManager.currentInstance.SendClickFromInventoryElement(wd_stored, index);
    }
}

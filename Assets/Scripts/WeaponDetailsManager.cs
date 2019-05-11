using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailsManager : MonoBehaviour {


    public bool AllowEquip = true;
    public bool AllowSell = true;
    public bool AllowUpgrade = true;
    private bool allowInput = false;
    private WeaponData wdOnDisplay;
    [Header("HUD References")]
    public CanvasGroup detailsParentCG;
    public Image detailsBackgroundCryo;
    public Image detailsBackgroundPhoton;
    public Image detailsBackgroundPulse;
    public Image detailsBackgroundElectric;
    public Image detailsBackgroundNuclear;
    public Image detailsBackgroundPlasma;
    public Image detailsBackgroundGamma;
    public Image detailsQualityBorder1;
    public Image detailsQualityBorder2;
    public Text detailsWeaponName;
    public Text detailsWeaponLevel;
    public Text detailsWeaponDmg;
    public Text detailsCritC;
    public Text detailsCritX;
    public Text detailsFirerate;
    public Text detailsHeat;
    public Text details_elementBonus;
    public Text details_elementPenalty;
    public Text detailsTrait;
    public Transform detailsDescElementPositive;
    public Transform detailsDescElementNegative;
    public Transform detailsDescTraits;


    public void OpenDetailsPanel(WeaponData wd)
    {
        if (wd == null)
        {
            detailsParentCG.gameObject.SetActive(false);
            return;
        }

        detailsParentCG.gameObject.SetActive(true);
        StopCoroutine("OpenMenuAnimation");
        StartCoroutine("OpenMenuAnimation");

        detailsDescElementPositive.gameObject.SetActive(false);
        detailsDescElementNegative.gameObject.SetActive(false);
        detailsDescTraits.gameObject.SetActive(true); //TEMP
        detailsTrait.text = wd.GetWeaponProjectileTrajectory().ToString();
        

        wdOnDisplay = wd;

        detailsBackgroundCryo.gameObject.SetActive(wd.GetWeaponElement() == WeaponData.DamageElement.cryo);
        detailsBackgroundPhoton.gameObject.SetActive(wd.GetWeaponElement() == WeaponData.DamageElement.photon);
        detailsBackgroundPulse.gameObject.SetActive(wd.GetWeaponElement() == WeaponData.DamageElement.pulse);
        detailsBackgroundElectric.gameObject.SetActive(wd.GetWeaponElement() == WeaponData.DamageElement.electric);
        detailsBackgroundNuclear.gameObject.SetActive(wd.GetWeaponElement() == WeaponData.DamageElement.nuclear);
        detailsBackgroundPlasma.gameObject.SetActive(wd.GetWeaponElement() == WeaponData.DamageElement.plasma);
        detailsBackgroundGamma.gameObject.SetActive(wd.GetWeaponElement() == WeaponData.DamageElement.gamma);

        detailsWeaponName.text = wd.GetWeaponName();
        detailsWeaponLevel.text = wd.GetUpgradeLevel().ToString();
        detailsWeaponDmg.text = wd.GetWeaponDamage().ToString() + "x" + wd.GetMultishoot().ToString();

        switch (wd.GetWeaponRarity())
        {
            case WeaponData.Rarity.rare:
                {
                    detailsQualityBorder1.color = detailsQualityBorder2.color = detailsWeaponName.color = GlobalGameManager.currentInstance.GetQualityColorRare();
                    break;
                }
            case WeaponData.Rarity.exotic:
                {
                    detailsQualityBorder1.color = detailsQualityBorder2.color = detailsWeaponName.color = GlobalGameManager.currentInstance.GetQualityColorExotic();
                    break;
                }
            case WeaponData.Rarity.hiTech:
                {
                    detailsQualityBorder1.color = detailsQualityBorder2.color = detailsWeaponName.color = GlobalGameManager.currentInstance.GetQualityColorHiTech();
                    break;
                }
            case WeaponData.Rarity.prototype:
                {
                    detailsQualityBorder1.color = detailsQualityBorder2.color = detailsWeaponName.color = GlobalGameManager.currentInstance.GetQualityColorPrototype();
                    break;
                }
            default:
                {
                    detailsQualityBorder1.color = detailsQualityBorder2.color = detailsWeaponName.color = GlobalGameManager.currentInstance.GetQualityColorCommon();
                    break;
                }
        }

        switch (wd.GetWeaponElement())
        {
            case WeaponData.DamageElement.photon:
                {
                    detailsDescElementPositive.gameObject.SetActive(true);
                    details_elementBonus.text = "Deals " + ((wd.GetWeaponDamage()*2f).ToString()) + " extra damage over 10 seconds. Multiple instances of this effect can be stacked. \nBullets will pierce through enemies once.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.electric:
                {
                    detailsDescElementPositive.gameObject.SetActive(true);
                    details_elementBonus.text = "Weapon impacts will bounce between close enemies up to 5 times.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.cryo:
                {
                    detailsDescElementPositive.gameObject.SetActive(true);
                    details_elementBonus.text = "Each bullet slows down enemies by " + (wd.GetWeaponDamage()*0.1f).ToString("F1") + "%, after reaching 50% slow, enemies will be stunnned and will take 100% increased damage for 5 seconds. \nBullets will bounce off walls and enemies once.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.gamma:
                {
                    detailsDescElementPositive.gameObject.SetActive(true);
                    details_elementBonus.text = "Projectiles release a gamma burst after impact";
                    details_elementPenalty.text = "Heats up extremely fast.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.nuclear:
                {
                    detailsDescElementPositive.gameObject.SetActive(true);
                    details_elementBonus.text = "Projectiles will explode on impact doing area damage.";
                    details_elementPenalty.text = "Can not critically hit.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.plasma:
                {
                    detailsDescElementPositive.gameObject.SetActive(true);
                    details_elementBonus.text = "Lowers enemy deffense, increasing their damage taken, based on damage done.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            default:
                {
                    detailsDescElementPositive.gameObject.SetActive(true);
                    details_elementBonus.text = "This weapon has no elemental effects, but it has balanced stats.";
                    detailsDescElementNegative.gameObject.SetActive(false);
                    break;
                }
        }
        detailsFirerate.text = wd.GetFirerate().ToString("F1") + "/sec";
        if (wd.GetWeaponElement() != WeaponData.DamageElement.nuclear)
        {
            detailsCritC.text = wd.GetCritChance().ToString("F0") + "%";
            detailsCritX.text = "X " + wd.GetCritMultiplier().ToString("F1");
        }
        else {
            detailsCritC.text = "N/A";
            detailsCritX.text = "N/A";
        }
        detailsHeat.text = wd.GetHeatPerProjectile().ToString() + "x" + wd.GetMultishoot();

    }
    IEnumerator OpenMenuAnimation() {
        allowInput = false;

        float t = 0;
        float animSpeed = 8;
        detailsParentCG.alpha = 0;
        detailsParentCG.transform.localScale = Vector3.one * 0.7f;
        while (t < 1) {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed);
            detailsParentCG.alpha = t;
            detailsParentCG.transform.localScale = Vector3.one * ((t * 0.3f) + 0.5f);
            yield return null;
        }
        allowInput = true;

        // Soluciona el error de posicionamiento de los detalles de arma, solo funciona aqui, abandono intentando entenderlo.
        detailsParentCG.gameObject.SetActive(false);
        detailsParentCG.gameObject.SetActive(true);
    }
    IEnumerator CloseMenuAnimation() {
        allowInput = false;
        float t = 1;
        float animSpeed = 8;
        detailsParentCG.alpha = 0;
        detailsParentCG.transform.localScale = Vector3.one * 0.7f;
        while (t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime * animSpeed);
            detailsParentCG.alpha = t;
            detailsParentCG.transform.localScale = Vector3.one * ((t * 0.3f) + 0.5f);
            yield return null;
        }
        detailsParentCG.gameObject.SetActive(false);
    }
    #region On Button Press
    public void OnUpgradeButtonClicked()
    {
        if (!allowInput)
            return;
    }
    public void OnSellButtonClicked()
    {
        if (!allowInput)
            return;
    }
    public void OnEquipButtonClicked()
    {
        GlobalGameManager.currentInstance.GetGameData().SetNewPlayerSelectedWeapon(wdOnDisplay);
        if (!allowInput)
            return;
    }
    public void OnCloseDetailsClicked()
    {
        if (!allowInput)
            return;
        StartCoroutine("CloseMenuAnimation");
    }
    #endregion
}

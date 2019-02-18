using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryManager : MonoBehaviour {

    public static WeaponInventoryManager currentInstance;

    public List<WeaponInventoryElement> inventory_elements;
    [Header("Texts")]
    public Text inventorySizeText;
    public Text pageNumberText;
    [Header("CGs")]
    public CanvasGroup inventoryElementsCG;
    public CanvasGroup detailsParentCG;
    [Header("Details Panel")]
    public Text detailsWeaponName;
    public Text detailsWeaponLevel;
    public Text detailsWeaponDmg;
    public Transform detailsBackgroundPhoton;
    public Transform detailsBackgroundPulse;
    public Transform detailsBackgroundElectric;
    public Transform detailsBackgroundCryo;
    public Transform detailsBackgroundNuclear;
    public Transform detailsBackgroundPlasma;
    public Transform detailsBackgroundGamma;
    public Transform detailsDescElementNegative;
    public Transform detailsDescTraits;
    public Text details_basic;
    public Text details_elementBonus;
    public Text details_elementPenalty;
    public Text details_traits;

    private bool swapPageAnimationInProgress = false;
    private bool detailsOpen = false;

    int page = 0;

    private void Awake()
    {
        currentInstance = this;
    }
    public void UpdatePageNumberAndInventorySize() {
        pageNumberText.text = "Page " + (page+1) + "/" + (((GlobalGameManager.currentInstance.GetGameData().GetAmountOfWeaponsInInventory()-1) / inventory_elements.Count) +1);
        inventorySizeText.text = GlobalGameManager.currentInstance.GetGameData().GetAmountOfWeaponsInInventory() + " items in inventory.";
    }
    public void OpenMenu() {
        page = 0;
        UpdateAllElements();
        gameObject.SetActive(true);
    }
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
    private void UpdateAllElements() {
        UpdatePageNumberAndInventorySize();
        for (int i = 0; i < inventory_elements.Count; i++)
        {
            inventory_elements[i].SetFor(GlobalGameManager.currentInstance.GetGameData().GetWeaponInInventorySlot(page * inventory_elements.Count + i));
        }
    }
    public void OnNextPageButtonClicked() {
        if (swapPageAnimationInProgress || detailsOpen)
            return;
        if ((GlobalGameManager.currentInstance.GetGameData().GetAmountOfWeaponsInInventory()-1)/inventory_elements.Count > page) {
            StartCoroutine("NextPageAnimation");
        }

    }
    public void OpenDetailsPanel(WeaponData wd) {
        detailsParentCG.gameObject.SetActive(true);
        detailsOpen = true;

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

        switch (wd.GetWeaponElement()) {
            case WeaponData.DamageElement.photon:
                {
                    details_elementBonus.text = "Leaves a stacking damage over time effect on enemies based on the damage done.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.electric:
                {
                    details_elementBonus.text = "Weapon impacts will bounce between enemies up to 4 times.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.cryo:
                {
                    details_elementBonus.text = "Slows down enemies based on the damage done, after reaching 50% slow, enemies will be stunnned and will take 100% increased damage for 5 seconds.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.gamma:
                {
                    details_elementBonus.text = "30% Chance to release a gamma ray burst on impact, dealing 300% of the bullet damage.";
                    details_elementPenalty.text = "Can not critically hit.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.nuclear:
                {
                    details_elementBonus.text = "Projectiles will explode on impact doing area damage.";
                    details_elementPenalty.text = "Can not critically hit.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.plasma:
                {
                    details_elementBonus.text = "Lowers enemy deffense, increasing their damage taken, based on damage done.";
                    details_elementPenalty.text = "Deals reduced direct damage.";
                    detailsDescElementNegative.gameObject.SetActive(true);
                    break;
                }
            default:
                {
                    details_elementBonus.text = "This weapon has no elemental effects, but it has balanced stats.";
                    detailsDescElementNegative.gameObject.SetActive(false);
                    break;
                }
        }
        details_basic.text = "Firerate: " + wd.GetFirerate().ToString("F1") + "shoots per second";
        if (wd.GetWeaponElement() != WeaponData.DamageElement.nuclear && wd.GetWeaponElement() != WeaponData.DamageElement.gamma)
        {
            details_basic.text += "\nCritical chance: " + wd.GetCritChance().ToString("F0") + "%";
            details_basic.text += "\nCritical damage: x" + wd.GetCritMultiplier().ToString("F1");
        }
        detailsDescTraits.gameObject.SetActive(false);

    }
    public void OnSortButtonClicked() {
        if (swapPageAnimationInProgress)
            return;

        //GlobalGameManager.currentInstance.GetGameData().SortWeaponInventoryByLevel();
        //GlobalGameManager.currentInstance.GetGameData().SortWeaponInventoryByElement();
        GlobalGameManager.currentInstance.GetGameData().SortWeaponInventoryByRarity();
        UpdateAllElements();
    }
    public void OnCloseInventoryClicked() {
        if (swapPageAnimationInProgress || detailsOpen)
            return;
        if (!gameObject.activeInHierarchy)
            return;
        CloseMenu();
        MainMenuManager.currentInstance.OnExitInventoryClick();
    }
    public void SendClickFromInventoryElement(WeaponData WD, int index) {
        OpenDetailsPanel(WD);
        GlobalGameManager.currentInstance.GetGameData().SetNewPlayerSelectedWeapon(WD);
        UpdateAllElements();
    }
    public void OnPreviousPageButtonClicked() {
        if (swapPageAnimationInProgress)
            return;
        if (page > 0) {
            StartCoroutine("PreviousPageAnimation");
        }
    }
    public void OnCloseDetailsClicked()
    {
        detailsOpen = false;
        detailsParentCG.gameObject.SetActive(false);
    }
    IEnumerator NextPageAnimation() {
        swapPageAnimationInProgress = true;
        float t = 0;
        float animspeed = 10;
        while (t < 1) {
            t += Time.deltaTime * animspeed;
            inventoryElementsCG.alpha = 1 - t;
            inventoryElementsCG.transform.localPosition = new Vector3(-t*80, inventoryElementsCG.transform.localPosition.y, inventoryElementsCG.transform.localPosition.z);
            yield return null;
        }
        t = 1;
        inventoryElementsCG.alpha = 0;
        page++;
        UpdateAllElements();
        while (t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime* animspeed);
            inventoryElementsCG.alpha = 1 - t;
            inventoryElementsCG.transform.localPosition = new Vector3(t * 80, inventoryElementsCG.transform.localPosition.y, inventoryElementsCG.transform.localPosition.z);
            yield return null;
        }
        swapPageAnimationInProgress = false;
    }
    IEnumerator PreviousPageAnimation()
    {
        swapPageAnimationInProgress = true;
        float t = 0;
        float animspeed = 10;
        while (t < 1)
        {
            t += Time.deltaTime * animspeed;
            inventoryElementsCG.alpha = 1 - t;
            inventoryElementsCG.transform.localPosition = new Vector3(t * 80, inventoryElementsCG.transform.localPosition.y, inventoryElementsCG.transform.localPosition.z);
            yield return null;
        }
        t = 1;
        inventoryElementsCG.alpha = 0;
        page--;
        UpdateAllElements();
        while (t > 0)
        {
            t -= Time.deltaTime * animspeed;
            inventoryElementsCG.alpha = 1 - t;
            inventoryElementsCG.transform.localPosition = new Vector3(-t * 80, inventoryElementsCG.transform.localPosition.y, inventoryElementsCG.transform.localPosition.z);
            yield return null;
        }
        swapPageAnimationInProgress = false;
    }

}

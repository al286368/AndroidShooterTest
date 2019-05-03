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
    [Header("Other")]
    public CanvasGroup inventoryElementsCG;
    public WeaponDetailsManager WDM;

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
    public void OnPreviousPageButtonClicked()
    {
        if (swapPageAnimationInProgress)
            return;
        if (page > 0)
        {
            StartCoroutine("PreviousPageAnimation");
        }
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
        WDM.OpenDetailsPanel(WD);
        UpdateAllElements();
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

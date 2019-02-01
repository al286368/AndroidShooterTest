using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameHudManager : MonoBehaviour {

    private List<NotificationBehaviour> notificationPool;
    private int notificationPoolInitialSize = 20;

    [Header("Canvas Groups")]
    public CanvasGroup LevelIntroCG;
    public CanvasGroup LevelEndCG;
    public CanvasGroup LevelEndFadeCG;
    [Header("Notifications")]
    public GameObject notificationPrefab;
    public Transform notificationParent;
    [Header("HUD References")]
    public Image slider_energy;
    public Image slider_shield;
    public Image slider_shieldRecovery;
    public Image slider_special;
    public Image slider_heat;
    public Image slider_heatRecovery;
    public CanvasGroup heatBlink;
    public CanvasGroup specialBlink;
    public Text text_health;
    public Text text_shield;
    public Text text_special;
    public Text text_heat;
    public Text text_overheat;
    public Text text_specialready;
    public Text text_levelEndInfo;
    [Header("Entity References")]
    public PlayerEntity player;

    public static IngameHudManager currentInstance;

    private float animation_ShieldPercent = 0;
    private float animation_EnergyPercent = 0;
    private float animation_ShieldAmount = 0;
    private float animation_EnergyAmount = 0;

    private const float ANIMATION_BAR_SPEED = 2;
    private const float ANIMATION_NUMBER_SPEED = 200;

    private void Awake()
    {
        currentInstance = this;
        InitializePool();
    }
    private void Start()
    {
        StartCoroutine("HeatWarningBlink");
        StartCoroutine("SpecialWarningBlink");
    }
    public void DisplayLevelIntro() {
        StopCoroutine("HUD_LevelIntro");
        StartCoroutine("HUD_LevelIntro");
    }
    public void DisplayLevelEnd(bool victory) {
        if (victory) {
            text_levelEndInfo.text = "Stage Completed";
        } else {
            text_levelEndInfo.text = "Stage Failed";
        }
        StartCoroutine("HUD_LevelEnd");
    }
    private void Update()
    {
        UpdateEnergyBar();
        UpdateShieldBar();
        UpdateSpecialBar();
        UpdateHeatBar();
    }
    #region HUD Update
    void UpdateEnergyBar() {
        animation_EnergyPercent = Mathf.MoveTowards(animation_EnergyPercent, player.GetHealthPercent() * 0.75f, Time.deltaTime * ANIMATION_BAR_SPEED);
        slider_energy.fillAmount = animation_EnergyPercent;
        animation_EnergyAmount = Mathf.MoveTowards(animation_EnergyAmount, player.GetCurrentHealth(), Time.deltaTime * ANIMATION_NUMBER_SPEED);
        text_health.text = ((int)animation_EnergyAmount).ToString();
    }
    void UpdateShieldBar() {
        animation_ShieldPercent = Mathf.MoveTowards(animation_ShieldPercent, player.GetShieldPercent() * 0.75f, Time.deltaTime * ANIMATION_BAR_SPEED);
        slider_shield.fillAmount = animation_ShieldPercent;
        animation_ShieldAmount = Mathf.MoveTowards(animation_ShieldAmount, player.GetCurrentShield(), Time.deltaTime * ANIMATION_NUMBER_SPEED);
        text_shield.text = ((int)animation_ShieldAmount).ToString();
        slider_shieldRecovery.fillAmount = player.GetShieldRecoveryPercent() * 0.75f;
    }
    void UpdateHeatBar() {
        slider_heat.fillAmount = player.GetWeaponHeat()/100;
        text_overheat.enabled = player.IsOverheated();
    }
    void UpdateSpecialBar() {
        slider_special.fillAmount = player.GetSpecialCharge() / 100;
        text_specialready.enabled = player.SpecialReady();
    }
    #endregion
    #region Notification Pool And Management
    private void InitializePool()
    {
        notificationPool = new List<NotificationBehaviour>();
        GameObject lastCreatedNotification;
        for (int i = 0; i < notificationPoolInitialSize; i++)
        {
            lastCreatedNotification = Instantiate(notificationPrefab, notificationParent) as GameObject;
            lastCreatedNotification.gameObject.SetActive(false);
            notificationPool.Add(lastCreatedNotification.GetComponent<NotificationBehaviour>());
        }
    }
    private NotificationBehaviour GetNotificationFromPool()
    {
        for (int i = 0; i < notificationPool.Count; i++)
        {
            if (!notificationPool[i].gameObject.activeInHierarchy)
            {
                return notificationPool[i];
            }
        }
        GameObject lastCreatedNotification;
        lastCreatedNotification = Instantiate(notificationPrefab, notificationParent) as GameObject;
        lastCreatedNotification.gameObject.SetActive(false);
        notificationPool.Add(lastCreatedNotification.GetComponent<NotificationBehaviour>());
        return lastCreatedNotification.GetComponent<NotificationBehaviour>();
    }
    public void DisplayStatusNotification(Enums.StatusEffect effect, Vector3 worldpos)
    {
        GetNotificationFromPool().SetAsStatus(effect, worldpos);
    }
    public void DisplayDamageNotification(float dmg, bool crit, Enums.DamageType type, Vector3 worldpos)
    {
        GetNotificationFromPool().SetAsDamage(dmg, crit, type, worldpos);
    }
    #endregion
    #region HUD Animation Co-Routines
    IEnumerator HeatWarningBlink() {
        float t = 0;
        float blinkSpeed = 3;
        while (true) {
            if (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.deltaTime * blinkSpeed);
                heatBlink.transform.localScale = new Vector3(1 + t * 0.2f, 1 + t, 1);
                heatBlink.alpha = (1 - t) * 0.6f;
            }
            else {
                heatBlink.alpha = 0;
                if (player.GetWeaponHeat() > 65 || player.IsOverheated()) {
                    t = 0;
                    yield return new WaitForSeconds(0.25f);
                }
            }

            yield return null;
        }
    }
    IEnumerator SpecialWarningBlink() {
        float t = 0;
        float blinkSpeed = 3;
        while (true)
        {
            if (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.deltaTime * blinkSpeed);
                specialBlink.transform.localScale = new Vector3(1 + t * 0.2f, 1 + t, 1);
                specialBlink.alpha = (1 - t) * 0.6f;
            }
            else
            {
                specialBlink.alpha = 0;
                if (player.GetSpecialCharge() >= 100)
                {
                    t = 0;
                    yield return new WaitForSeconds(0.25f);
                }
            }

            yield return null;
        }
    }
    IEnumerator HUD_LevelIntro()
    {
        float t = 0;
        float animSpeed = 1;

        LevelIntroCG.gameObject.SetActive(true);
        LevelIntroCG.alpha = 0;
        while (t < 1)
        {
            t += Time.deltaTime * animSpeed;
            LevelIntroCG.alpha = t;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        animSpeed = 3;
        while (t > 0)
        {
            t -= Time.deltaTime * animSpeed;
            LevelIntroCG.alpha = t;
            LevelIntroCG.transform.localScale = new Vector3(1, 1, 1) * (1 + (1 - t));
            yield return null;
        }
        LevelIntroCG.gameObject.SetActive(false);
    }
    IEnumerator HUD_LevelEnd() {
        float t = 0;
        float animSpeed = 1;
        LevelEndCG.gameObject.SetActive(true);
        LevelEndFadeCG.gameObject.SetActive(true);
        LevelEndFadeCG.alpha = LevelEndCG.alpha = 0;
        while (t < 1) {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed);
            LevelEndCG.alpha = t;
            yield return null;
        }
        t = 0;
        yield return new WaitForSeconds(2f);
        while (t < 1) {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed);
            LevelEndFadeCG.alpha = t;
            yield return null;
        }

    }
    #endregion
}

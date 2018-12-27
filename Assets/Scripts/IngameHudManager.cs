using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameHudManager : MonoBehaviour {

    private List<NotificationBehaviour> notificationPool;
    private int notificationPoolInitialSize = 20;

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
    public Text text_health;
    public Text text_shield;
    public Text text_special;
    public Text text_heat;
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
    private void Update()
    {
        UpdateEnergyBar();
        UpdateShieldBar();
        UpdateSpecialBar();
        UpdateHeatBar();
    }
    void UpdateEnergyBar() {
        animation_EnergyPercent = Mathf.MoveTowards(animation_EnergyPercent, player.GetHealthPercent(), Time.deltaTime * ANIMATION_BAR_SPEED);
        slider_energy.fillAmount = animation_EnergyPercent;
        animation_EnergyAmount = Mathf.MoveTowards(animation_EnergyAmount, player.GetCurrentHealth(), Time.deltaTime * ANIMATION_NUMBER_SPEED);
        text_health.text = ((int)animation_EnergyAmount).ToString();
    }
    void UpdateShieldBar() {
        animation_ShieldPercent = Mathf.MoveTowards(animation_ShieldPercent, player.GetShieldPercent(), Time.deltaTime * ANIMATION_BAR_SPEED);
        slider_shield.fillAmount = animation_ShieldPercent;
        animation_ShieldAmount = Mathf.MoveTowards(animation_ShieldAmount, player.GetCurrentShield(), Time.deltaTime * ANIMATION_NUMBER_SPEED);
        text_shield.text = ((int)animation_ShieldAmount).ToString();
        slider_shieldRecovery.fillAmount = player.GetShieldRecoveryPercent();
    }
    void UpdateHeatBar() {
    }
    void UpdateSpecialBar() {
        
    }
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
    public void DisplayDamageNotification(float dmg, Enums.DamageType type, Vector3 worldpos)
    {
        GetNotificationFromPool().SetAsDamage(dmg, type, worldpos);
    }
}

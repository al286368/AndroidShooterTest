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

    public static IngameHudManager currentInstance;

    private void Awake()
    {
        currentInstance = this;
        InitializePool();
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
    public void DisplayNotification(float dmg, Enums.DamageType type, Vector3 worldpos)
    {
        GetNotificationFromPool().SetAs(dmg, type, worldpos);
    }
}

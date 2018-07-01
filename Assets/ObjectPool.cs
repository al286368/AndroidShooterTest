using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject prefab_bullets;
    [Header("Object Group Parent")]
    public Transform parent_bullets;

    private List<BulletBehaviour> pool_bullets;

    private const int BASE_POOL_SIZE = 20;

    public static ObjectPool currentInstance;

    #region Setup
    private void Awake()
    {
        currentInstance = this;
        InitializeObjectPools();
    }
    private void InitializeObjectPools()
    {
        GameObject lastInstantiatedObject;
        pool_bullets = new List<BulletBehaviour>();

        for (int i = 0; i < BASE_POOL_SIZE; i++)
        {
            lastInstantiatedObject = Instantiate(prefab_bullets, parent_bullets) as GameObject;
            lastInstantiatedObject.gameObject.SetActive(false);
            pool_bullets.Add(lastInstantiatedObject.GetComponent<BulletBehaviour>());
        }
    }
    #endregion

    #region GetFromPool
    public BulletBehaviour GetBulletFromPool()
    {
        for (int i = 0; i < pool_bullets.Count; i++)
        {
            if (!pool_bullets[i].gameObject.activeInHierarchy)
            {
                return pool_bullets[i];
            }
        }
        GameObject lastInstantiatedObject = Instantiate(prefab_bullets, parent_bullets) as GameObject;
        lastInstantiatedObject.gameObject.SetActive(false);
        pool_bullets.Add(lastInstantiatedObject.GetComponent<BulletBehaviour>());
        return lastInstantiatedObject.GetComponent<BulletBehaviour>();
    }
    #endregion
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject prefab_bullets;
    public GameObject prefab_beams;
    public GameObject prefab_explosions;
    [Header("Object Group Parent")]
    public Transform parent_bullets;
    public Transform parent_beams;
    public Transform parent_explosions;

    private List<BulletBehaviour> pool_bullets;
    private List<BeamBehaviour> pool_beams;

    private const int BASE_POOL_SIZE = 10;

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

        pool_beams = new List<BeamBehaviour>();
        for (int i = 0; i < BASE_POOL_SIZE; i++)
        {
            lastInstantiatedObject = Instantiate(prefab_beams, parent_beams) as GameObject;
            lastInstantiatedObject.gameObject.SetActive(false);
            pool_beams.Add(lastInstantiatedObject.GetComponent<BeamBehaviour>());
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
    public BeamBehaviour GetBeamFromPool()
    {
        for (int i = 0; i < pool_beams.Count; i++)
        {
            if (!pool_beams[i].gameObject.activeInHierarchy)
            {
                return pool_beams[i];
            }
        }
        GameObject lastInstantiatedObject = Instantiate(prefab_beams, parent_beams) as GameObject;
        lastInstantiatedObject.gameObject.SetActive(false);
        pool_beams.Add(lastInstantiatedObject.GetComponent<BeamBehaviour>());
        return lastInstantiatedObject.GetComponent<BeamBehaviour>();
    }
    #endregion
}
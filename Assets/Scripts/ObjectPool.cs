using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject prefab_bullets;
    public GameObject prefab_beams;
    public GameObject prefab_explosions;
    public GameObject prefab_sparks;
    public GameObject prefab_gamma;
    [Header("Object Group Parent")]
    public Transform parent_attacks;
    public Transform parent_beams;

    private List<BulletBehaviour> pool_bullets;
    private List<ElectricSparkBehaviour> pool_sparks;
    private List<BeamBehaviour> pool_beams;
    private List<ExplosionBehaviour> pool_explosions;
    private List<GammaBeamBehaviour> pool_gamma;

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
            lastInstantiatedObject = Instantiate(prefab_bullets, parent_attacks) as GameObject;
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
        pool_sparks = new List<ElectricSparkBehaviour>();
        for (int i = 0; i < BASE_POOL_SIZE; i++)
        {
            lastInstantiatedObject = Instantiate(prefab_sparks, parent_attacks) as GameObject;
            lastInstantiatedObject.gameObject.SetActive(false);
            pool_sparks.Add(lastInstantiatedObject.GetComponent<ElectricSparkBehaviour>());
        }
        pool_explosions = new List<ExplosionBehaviour>();
        for (int i = 0; i < BASE_POOL_SIZE; i++)
        {
            lastInstantiatedObject = Instantiate(prefab_explosions, parent_attacks) as GameObject;
            lastInstantiatedObject.gameObject.SetActive(false);
            pool_explosions.Add(lastInstantiatedObject.GetComponent<ExplosionBehaviour>());
        }
        pool_gamma = new List<GammaBeamBehaviour>();
        for (int i = 0; i < BASE_POOL_SIZE; i++)
        {
            lastInstantiatedObject = Instantiate(prefab_gamma, parent_attacks) as GameObject;
            lastInstantiatedObject.gameObject.SetActive(false);
            pool_gamma.Add(lastInstantiatedObject.GetComponent<GammaBeamBehaviour>());
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
        GameObject lastInstantiatedObject = Instantiate(prefab_bullets, parent_attacks) as GameObject;
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
    public ElectricSparkBehaviour GetSparkFromPool()
    {
        for (int i = 0; i < pool_sparks.Count; i++)
        {
            if (!pool_sparks[i].gameObject.activeInHierarchy)
            {
                return pool_sparks[i];
            }
        }
        GameObject lastInstantiatedObject = Instantiate(prefab_sparks, parent_attacks) as GameObject;
        lastInstantiatedObject.gameObject.SetActive(false);
        pool_sparks.Add(lastInstantiatedObject.GetComponent<ElectricSparkBehaviour>());
        return lastInstantiatedObject.GetComponent<ElectricSparkBehaviour>();
    }
    public ExplosionBehaviour GetExplosionFromPool()
    {
        for (int i = 0; i < pool_explosions.Count; i++)
        {
            if (!pool_explosions[i].gameObject.activeInHierarchy)
            {
                return pool_explosions[i];
            }
        }
        GameObject lastInstantiatedObject = Instantiate(prefab_explosions, parent_attacks) as GameObject;
        lastInstantiatedObject.gameObject.SetActive(false);
        pool_explosions.Add(lastInstantiatedObject.GetComponent<ExplosionBehaviour>());
        return lastInstantiatedObject.GetComponent<ExplosionBehaviour>();
    }
    public GammaBeamBehaviour GetGammaBeamFromPool()
    {
        for (int i = 0; i < pool_gamma.Count; i++)
        {
            if (!pool_gamma[i].gameObject.activeInHierarchy)
            {
                return pool_gamma[i];
            }
        }
        GameObject lastInstantiatedObject = Instantiate(prefab_gamma, parent_attacks) as GameObject;
        lastInstantiatedObject.gameObject.SetActive(false);
        pool_gamma.Add(lastInstantiatedObject.GetComponent<GammaBeamBehaviour>());
        return lastInstantiatedObject.GetComponent<GammaBeamBehaviour>();
    }
    #endregion
}

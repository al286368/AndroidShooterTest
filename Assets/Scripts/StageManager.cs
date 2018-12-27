using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    public float BOUNDS_MIN_X;
    public float BOUNDS_MAX_X;

    private List<EntityNPC> enemyPool;
    private List<EntityNPC> enemiesInStage;
    private IEntity playerEntity;

    public static StageManager currentInstance;

    public float difficultyBulletTimeScaleFactor = 0.3f;

    private void Awake()
    {
        currentInstance = this;
        enemiesInStage = new List<EntityNPC>();
        enemyPool = new List<EntityNPC>();
    }
    public void RegisterEnemy(EntityNPC e)
    {
        if (!enemiesInStage.Contains(e))
            enemiesInStage.Add(e);
    }
    public void UnRegisterEnemy(EntityNPC e) {
        if (enemiesInStage.Contains(e))
            enemiesInStage.Remove(e);
    }
    public IEntity GetRandomEnemy()
    {
        if (enemiesInStage == null || enemiesInStage.Count == 0)
            return null;
        return enemiesInStage[Random.Range(0, enemiesInStage.Count)];
    }
    public void RegisterPlayerEntity(IEntity player) {
        playerEntity = player;
    }
    public IEntity GetPlayer()
    {
        return playerEntity;
    }
    public void SpawnDrifter(Vector3 pos) {
        EntityNPC NPCSpawned = null;
        for (int i = 0; i < enemyPool.Count; i++) {
            if (enemyPool[i].recycleTag == EntityNPC.RecycleTag.drifter && !enemyPool[i].gameObject.activeInHierarchy) {
                NPCSpawned = enemyPool[i];
                continue;
            }
        }
        if (NPCSpawned == null)
        {
            GameObject createdobject = Instantiate(PrefabManager.currentInstance.enemy_drifter);
            NPCSpawned = createdobject.GetComponent<EntityNPC>();
            enemyPool.Add(NPCSpawned);
        }
        NPCSpawned.transform.position = pos;
        NPCSpawned.gameObject.SetActive(true);
        NPCSpawned.ResetEntity();
    }
    
}

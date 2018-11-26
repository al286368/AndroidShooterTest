using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    public float BOUNDS_MIN_X;
    public float BOUNDS_MAX_X;

    private List<EntityBase> enemiesInStage;
    private EntityBase playerEntity;

    public static StageManager currentInstance;

    private void Awake()
    {
        currentInstance = this;
    }
    public void RegisterEnemy(EntityBase EB)
    {
        if (enemiesInStage == null)
            enemiesInStage = new List<EntityBase>();

        enemiesInStage.Add(EB);
    }
    public EntityBase GetRandomEnemy()
    {
        if (enemiesInStage == null || enemiesInStage.Count == 0)
            return null;
        return enemiesInStage[Random.Range(0, enemiesInStage.Count)];
    }
    public void RegisterPlayerEntity(EntityBase player) {
        playerEntity = player;
    }
    public EntityBase GetPlayer()
    {
        return playerEntity;
    }
    
}

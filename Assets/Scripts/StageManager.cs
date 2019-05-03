using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour {

    private List<EntityNPC> enemyPool;
    private List<EntityNPC> enemiesInStage;
    private IEntity playerEntity;

    public static StageManager currentInstance;

    public float difficultyBulletTimeScaleFactor = 0.3f;
    private bool stageEnded = false;


    private void Awake()
    {
        currentInstance = this;
        enemiesInStage = new List<EntityNPC>();
        enemyPool = new List<EntityNPC>();
    }
    private void Start()
    {
        IngameHudManager.currentInstance.DisplayLevelIntro();
    }
    #region Entity Management
    public void RegisterEnemy(EntityNPC e)
    {
        if (!enemiesInStage.Contains(e))
            enemiesInStage.Add(e);
    }
    public void UnRegisterEnemy(EntityNPC e) {
        if (enemiesInStage.Contains(e))
            enemiesInStage.Remove(e);
    }
    public void EndStage(bool victory) {
        if (stageEnded)
            return;
        stageEnded = true;
        StartCoroutine("EndStageDelay");
        IngameHudManager.currentInstance.DisplayLevelEnd(victory);
    }
    public IEntity GetClosestRivalEntityToPoint(Vector3 pos, IEntity user, IEntity optionalIgnoredEntity = null) {
        if (user.IsAlly())
        {
            if (enemiesInStage.Count == 0)
                return null;

            IEntity targetFound = null;
            float minDistance = 5f;
            float tmpDist = 0;
            for (int i = 0; i < enemiesInStage.Count; i++) {
                tmpDist = Vector3.Distance(pos, enemiesInStage[i].transform.position);
                if (tmpDist < minDistance && enemiesInStage[i] != (object)optionalIgnoredEntity) { // Object???
                    minDistance = tmpDist;
                    targetFound = enemiesInStage[i];
                }
            }
            return targetFound;
        }
        else {
            return optionalIgnoredEntity == GetPlayer() ? null : GetPlayer();
        }
    }
    public IEntity GetLowestDeltaXTargetForBullet(bool isAlly, float bulletX) {
        if (isAlly)
        {
            IEntity lowestDeltaXTarget = null;
            if (enemiesInStage.Count > 0) {
                lowestDeltaXTarget = enemiesInStage[0];
            }
            for (int i = 1; i < enemiesInStage.Count; i++) {
                if (Mathf.Abs(enemiesInStage[i].transform.position.x - bulletX) < Mathf.Abs(lowestDeltaXTarget.GetGameObject().transform.position.x) - bulletX) {
                    lowestDeltaXTarget = enemiesInStage[i];
                }
            }
            return lowestDeltaXTarget;
        }
        else {
            return GetPlayer();
        }
    }
    public IEntity GetTargetForBullet(bool isAlly) {
        if (isAlly)
        {
            return GetRandomEnemy();
        }
        else {
            return GetPlayer();
        }
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
    public void SpawnBomber(Vector3 pos)
    {
        EntityNPC NPCSpawned = null;
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (enemyPool[i].recycleTag == EntityNPC.RecycleTag.bomber && !enemyPool[i].gameObject.activeInHierarchy)
            {
                NPCSpawned = enemyPool[i];
                continue;
            }
        }
        if (NPCSpawned == null)
        {
            GameObject createdobject = Instantiate(PrefabManager.currentInstance.enemy_bomber);
            NPCSpawned = createdobject.GetComponent<EntityNPC>();
            enemyPool.Add(NPCSpawned);
        }
        NPCSpawned.transform.position = pos;
        NPCSpawned.gameObject.SetActive(true);
        NPCSpawned.ResetEntity();
    }
    #endregion
    IEnumerator EndStageDelay() {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("MainMenu");
    }

}

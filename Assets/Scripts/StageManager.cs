using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    private List<EntityNPC> enemyPool;
    private List<EntityNPC> enemiesInStage;
    private IEntity playerEntity;

    public static StageManager currentInstance;

    public float difficultyBulletTimeScaleFactor = 0.3f;

    public CanvasGroup LevelIntroCG;


    private void Awake()
    {
        currentInstance = this;
        enemiesInStage = new List<EntityNPC>();
        enemyPool = new List<EntityNPC>();
    }
    private void Start()
    {
        DisplayLevelIntro();
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
    public IEntity GetClosestRivalEntityToPoint(Vector3 pos, IEntity user, IEntity optionalIgnoredEntity = null) {
        if (user.IsAlly())
        {
            if (enemiesInStage.Count == 0)
                return null;

            IEntity targetFound = null;
            float minDistance = 99999;
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
    #region Visual Notifications Management
    public void DisplayLevelIntro() {
        StopCoroutine("HUD_LevelIntro");
        StartCoroutine("HUD_LevelIntro");
    }
    IEnumerator HUD_LevelIntro()
    {
        float t = 0;
        float animSpeed = 1;

        LevelIntroCG.gameObject.SetActive(true);
        LevelIntroCG.alpha = 0;
        while (t < 1) {
            t += Time.deltaTime * animSpeed;
            LevelIntroCG.alpha = t;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        animSpeed = 3;
        while (t > 0) {
            t -= Time.deltaTime * animSpeed;
            LevelIntroCG.alpha = t;
            LevelIntroCG.transform.localScale = new Vector3(1,1,1) * (1+(1-t));
            yield return null;
        }
        LevelIntroCG.gameObject.SetActive(false);
    }
    #endregion

}

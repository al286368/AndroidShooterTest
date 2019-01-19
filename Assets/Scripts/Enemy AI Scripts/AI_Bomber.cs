using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Bomber : MonoBehaviour, IEnemyAI
{
    public EntityNPC entityManaged;
    IEntity player;
    private const float MIN_ENTRANCE_MOVESPEED = 1;
    private const float RETREAT_MOVESPEED = 10;
    private const float FIGHT_MOVESPEED = 3;
    private const float MAX_FIGHT_TIME = 10;
    private const float COMBAT_MAX_Y = 8;
    private const float COMBAT_MIN_Y = 3;

    private const float FIRERATE = 0.25f;

    private float lifetime = 0;
    private float shootready = 0;
    private float targetCombatY = 0;


    void FixedUpdate() {
        lifetime += Time.fixedDeltaTime * entityManaged.GetEntityTimescale();
        if (lifetime < MAX_FIGHT_TIME)
        {
            shootready += Time.fixedDeltaTime * entityManaged.GetEntityTimescale() * FIRERATE;
            if (transform.position.y > targetCombatY)
                transform.Translate(0, -((transform.position.y - targetCombatY) + MIN_ENTRANCE_MOVESPEED) * Time.fixedDeltaTime * entityManaged.GetEntityTimescale(), 0);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.GetGameObject().transform.position.x, transform.position.y, transform.position.z), FIGHT_MOVESPEED * Time.fixedDeltaTime * entityManaged.GetEntityTimescale());

        }
        else {
            transform.Translate(0, RETREAT_MOVESPEED * Time.fixedDeltaTime * entityManaged.GetEntityTimescale(), 0);
            if (transform.position.y > 17) {
                entityManaged.RetreatFromStage();
            }
        }
        if (shootready > 1)
        {
            shootready = 0;
            entityManaged.Shoot(270);
        }
    }
    public void NotifyDamageTaken(float dmg)
    {

    }

    public void ResetAI()
    {
        player = StageManager.currentInstance.GetPlayer();
        shootready = 0;
        lifetime = 0;
        targetCombatY = Random.Range(COMBAT_MIN_Y,COMBAT_MAX_Y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Drifter : MonoBehaviour, IEnemyAI
{

    private IEntity player;
    private Vector3 targetPos;

    public EntityNPC entityNPC;

    float shootready = 0;
    float movespeed = 20;
    float angleToTarget;

    private const float MAX_ALLOWED_X = 4;
    private const float MIN_ALLOWED_X = -4;
    private const float MAX_ALLOWED_Y = 9;
    private const float MIN_ALLOWED_Y = 4;
    private const float FIND_NEXT_POSITION_DELAY = 3f;
    private const float INITIAL_TRANSLATION_DELAY = 5f;
    private const float MAX_STARTING_Y = 6f;
    private const float MIN_STARTING_Y = 4.5f;
    private const float EVADE_SPEED = 30;
    private const float ESCAPE_SPEED = 15;

    private const float RETREAT_TIME = 10f;

    private float findNextPositionTimer = 0;
    private float lifetime = 0;


    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = StageManager.currentInstance.GetPlayer();
            angleToTarget = 0;
        }
        else
        {
            angleToTarget = TrackTo(player.GetGameObject().transform);
            if (entityNPC.GetEntityTimescale() > 0)
                transform.rotation = Quaternion.Euler(0,0, angleToTarget+90);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * movespeed * entityNPC.GetEntityTimescale());

        if (lifetime < RETREAT_TIME)
        {
            lifetime += Time.deltaTime * entityNPC.GetEntityTimescale();
            if (lifetime >= RETREAT_TIME)
            {
                movespeed = ESCAPE_SPEED;
                targetPos = new Vector3(Random.Range(1, 3) == 1 ? -12 : 12, transform.position.y, 0);
            }
        }
        else if (transform.position == targetPos)
        {
            entityNPC.RetreatFromStage();
        }

        shootready += Time.deltaTime * entityNPC.GetEntityTimescale();
        if (shootready > 1 && player != null)
        {
            shootready = 0;
            entityNPC.Shoot(angleToTarget);
        }
    }
    public void ResetAI()
    {
        lifetime = 0;
        movespeed = 5;
        targetPos = new Vector3(transform.position.x, Random.Range(MIN_STARTING_Y, MAX_STARTING_Y), 0);
    }
    public void NotifyDamageTaken(float dmg)
    {
        if (lifetime > RETREAT_TIME)
            return;
        if (transform.position == targetPos)
            targetPos = new Vector3(Random.Range(MIN_ALLOWED_X, MAX_ALLOWED_X), Random.Range(MIN_ALLOWED_Y, MAX_ALLOWED_Y), 0);
        movespeed = EVADE_SPEED;
    }

    float TrackTo(Transform targTransform)
    {
        if (targTransform == null)
            return 0;

        float difX = targTransform.position.x - transform.position.x;
        float difY = targTransform.position.y - transform.position.y;
        if (difX == 0)
        {
            if (difY > 0) { return 90; }
            else { return 270; }
        }

        if (difX > 0) { return Mathf.Atan(difY / difX) * Mathf.Rad2Deg; }
        else { return (Mathf.Atan(difY / difX) * Mathf.Rad2Deg) + 180; }
    }
}

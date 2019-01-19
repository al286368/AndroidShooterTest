using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Test : MonoBehaviour, IEnemyAI {

    private IEntity player;
    private Vector3 targetPos;

    public EntityNPC entityNPC;

    float shootready = 0;
    float movespeed = 20;

    private const float MAX_X = 4;
    private const float MIN_X = -4;
    private const float MAX_Y = 9;
    private const float MIN_Y = 4;
    private const float FIND_NEXT_POSITION_DELAY = 3f;

    private float findNextPositionTimer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            player = StageManager.currentInstance.GetPlayer();

        if (findNextPositionTimer < 0){
            targetPos = new Vector3(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y), 0);
            findNextPositionTimer = FIND_NEXT_POSITION_DELAY;
        }
        else {
            findNextPositionTimer -= Time.deltaTime * entityNPC.GetEntityTimescale();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * movespeed * entityNPC.GetEntityTimescale());

        shootready += Time.deltaTime * entityNPC.GetEntityTimescale();
        if (shootready > 1 && player != null)
        {
            shootready = 0;
            entityNPC.Shoot(TrackTo(player.GetGameObject().transform));
        }
	}
    public void ResetAI() {
        print("Reseting AI");
    }
    public void NotifyDamageTaken(float dmg)
    {
        findNextPositionTimer = 0;
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

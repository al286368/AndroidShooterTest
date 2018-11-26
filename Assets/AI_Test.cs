using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Test : MonoBehaviour, IEnemyAI {

    private EntityBase playerEntity;
    private Vector3 targetPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (playerEntity == null)
            StageManager.currentInstance.GetPlayer();
        else {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 2f);
        }
	}
    public void ResetAI() {
        print("Reseting AI");
    }
}

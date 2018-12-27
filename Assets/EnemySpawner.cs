using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject EnemyToSpawn;

	// Use this for initialization
	void Start () {
        StartCoroutine("testSecuence");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator testSecuence() {
        int waves = 10;
        while (waves > 0)
        {
            waves--;
            StageManager.currentInstance.SpawnDrifter(new Vector3(-4, 18, 0));
            StageManager.currentInstance.SpawnDrifter(new Vector3(4, 18, 0));
            yield return new WaitForSeconds(4);
        }

    }
}

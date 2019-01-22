using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("testSecuence");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator testSecuence() {
        int waves = 30;
        yield return new WaitForSeconds(1);
        while (waves > 0)
        {
            waves--;
            StageManager.currentInstance.SpawnBomber(new Vector3(0, 18, 0));
            StageManager.currentInstance.SpawnDrifter(new Vector3(-4, 18, 0));
            StageManager.currentInstance.SpawnDrifter(new Vector3(4, 18, 0));
            yield return new WaitForSeconds(3);
        }
        yield return new WaitForSeconds(10);
        StageManager.currentInstance.EndStage(true);
    }
}

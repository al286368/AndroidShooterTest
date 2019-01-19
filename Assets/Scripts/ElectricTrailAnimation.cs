using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrailAnimation : MonoBehaviour {

    public BulletBehaviour BB;
    private const float OSCILATION_RANGE = 0.1f;

	void Update () {
        if (!BB.IsWaitingToBeRemoved())
            transform.localPosition = new Vector3(Random.Range(-OSCILATION_RANGE, OSCILATION_RANGE), Random.Range(-OSCILATION_RANGE, OSCILATION_RANGE), 0);
	}
}

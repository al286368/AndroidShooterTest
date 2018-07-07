using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour {

	// Use this for initialization
	void Start () {



    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BeamBehaviour beam;
            beam = ObjectPool.currentInstance.GetBeamFromPool();
            beam.SetShape(transform.position, 5, 20, 0.2f);

            beam = ObjectPool.currentInstance.GetBeamFromPool();
            beam.SetShape(transform.position, 5, 45, 0.2f);

            beam = ObjectPool.currentInstance.GetBeamFromPool();
            beam.SetShape(transform.position, 5, 160, 0.2f);

            beam = ObjectPool.currentInstance.GetBeamFromPool();
            beam.SetShape(transform.position, 5, 135, 0.2f);
        }
    }
}

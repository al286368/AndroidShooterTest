using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenTest : MonoBehaviour {

    private Touch t;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        t = Input.GetTouch(0);
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(t.position).x,Camera.main.ScreenToWorldPoint(t.position).y,0);
    }
}

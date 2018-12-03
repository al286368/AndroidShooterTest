using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMultiTouch : MonoBehaviour {

    public int index;

	// Update is called once per frame
	void Update () {
        if (Input.touches.Length < index)
        {
            Touch t = Input.GetTouch(index);
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, 0);
        }

    }
}

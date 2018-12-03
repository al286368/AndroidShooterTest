using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedActionTest : MonoBehaviour {


    
	// Use this for initialization
	void Start () {
        StartCoroutine("DelayedDisable");
	}
	IEnumerator DelayedDisable()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}

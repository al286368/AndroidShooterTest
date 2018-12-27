using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBehaviourAlternative : MonoBehaviour {

    public Transform targetPoint;
    public Transform trailTransform;

    private IEntity entityUser;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetBeam(IEntity e_user, Vector2 basePos, float degree, Collider2D colliderToIgnore = null)
    {
        entityUser = e_user;
        transform.position = basePos;
    }
}

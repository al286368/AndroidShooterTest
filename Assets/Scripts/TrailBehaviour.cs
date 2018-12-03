using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehaviour : MonoBehaviour {

    public TrailRenderer TR;

    public void SetTrail(float tStartWidth, float tEndWidth, float tLenght, Color tColor, Transform tParent)
    {
        TR.startColor = TR.endColor = tColor;
        TR.endWidth = tEndWidth;
        TR.startWidth = tStartWidth;
        TR.time = tLenght;
        transform.position = tParent.position;
        transform.parent = tParent;
    }
}

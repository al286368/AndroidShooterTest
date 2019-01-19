using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVisualsManager : MonoBehaviour {

    public TrailRenderer TR;
    public ParticleSystem PS;


    public void SetTrail(bool isally)
    {
        TR.Clear();
    }
}

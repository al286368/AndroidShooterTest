using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    public float BOUNDS_MIN_X;
    public float BOUNDS_MAX_X;

    public static StageManager currentInstance;

    private void Awake()
    {
        currentInstance = this;
    }
    
}

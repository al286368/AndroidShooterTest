using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

    public GameObject enemy_drifter;

    public static PrefabManager currentInstance;

    private void Awake()
    {
        currentInstance = this;
    }
}

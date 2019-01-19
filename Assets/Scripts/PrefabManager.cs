using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

    public GameObject enemy_drifter;
    public GameObject enemy_bomber;

    public static PrefabManager currentInstance;

    private void Awake()
    {
        currentInstance = this;
    }
}

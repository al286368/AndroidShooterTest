using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

    public GameObject enemy_drifter;
    public GameObject enemy_bomber;

    public GameObject special_deflectPulse;

    public static PrefabManager currentInstance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentInstance = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDeflectPulse : MonoBehaviour, ISpecial {

    public SpriteRenderer SR;
    public DeflecterProperties DP;

    private const float EXPANSION_RATE = 3f;
    private const float EXPANSION_SPEED = 2.5f;
    private const float BASE_SCALE = 0.1f;

    private float t = 0;

    public void ActivateEffect(IEntity user)
    {
        DP.SetUser(user);
        transform.position = user.GetGameObject().transform.position;
        transform.localScale = Vector3.one * BASE_SCALE;
        gameObject.SetActive(true);
        t = 0;
    }
    void FixedUpdate() {
        t += Time.fixedDeltaTime * EXPANSION_SPEED;
        if (t > 1)
            gameObject.SetActive(false);
        transform.localScale = Vector3.one * (BASE_SCALE + t*EXPANSION_RATE);
        SR.color = new Color(1,1,1,1-t);

    }
    public GameObject GetGameObject() {
        return gameObject;
    }
}

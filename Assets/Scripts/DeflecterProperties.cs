using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflecterProperties : MonoBehaviour {

    public IEntity deflectUser;
    public float deflectStrenght;

    public IEntity GetUser() {
        return deflectUser;
    }
    public void SetUser(IEntity user) {
        deflectUser = user;
    }
    public float GetDeflectStrenght() {
        return deflectStrenght;
    }
}

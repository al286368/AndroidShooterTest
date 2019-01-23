using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecial {

    void ActivateEffect(IEntity user);
    GameObject GetGameObject();
}

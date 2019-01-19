using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAI
{
    void ResetAI();
    void NotifyDamageTaken(float dmg);
}

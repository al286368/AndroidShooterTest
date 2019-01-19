using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryNormal : MonoBehaviour, ITrajectory {

    private float degreeLocal;
    private float degreeParent;

    private BulletBehaviour bulletManaged;


    public void ResetTrajectory(BulletBehaviour bulletToManage, float local, float parent)
    {
        enabled = true;
        bulletManaged = bulletToManage;
        degreeLocal = local;
        degreeParent = parent;
    }


    void FixedUpdate()
    {
        bulletManaged.SetAngle(degreeLocal, degreeParent);
    }

    public void SendWallCollisionNotification(float normalAngle)
    {
        degreeParent += degreeLocal;
        degreeLocal = 0;

        float difN = 90 - normalAngle;
        degreeParent = (-(degreeParent - difN)) + difN;
        bulletManaged.SetAngle(degreeLocal, degreeParent);
    }

    public void SendEntityCollisionNotification()
    {
        degreeLocal = 0;
        degreeParent = Random.Range(0, 361);
    }
}

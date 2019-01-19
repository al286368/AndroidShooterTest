using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryHelix : MonoBehaviour, ITrajectory {

    private float degreeLocal;
    private float degreeParent;
    private float helixTarget;
    private BulletBehaviour bulletManaged;

    private const float HELIX_TURNRATE = 210;

    public void ResetTrajectory(BulletBehaviour bulletToManage, float local, float parent)
    {
        enabled = true;
        bulletManaged = bulletToManage;
        degreeLocal = local;
        degreeParent = parent;
        helixTarget = -local;
    }
    

    void FixedUpdate () {
        degreeLocal = Mathf.MoveTowards(degreeLocal, helixTarget, Time.fixedDeltaTime * bulletManaged.GetTimeScale() * HELIX_TURNRATE);
        if (degreeLocal == helixTarget)
        {
            helixTarget *= -1;
        }
        bulletManaged.SetAngle(degreeLocal, degreeParent);
    }

    public void SendWallCollisionNotification(float normalAngle)
    {
        degreeParent += degreeLocal;
        degreeLocal = 0;

        float difN = 90 - normalAngle;
        degreeParent = (-(degreeParent - difN)) + difN;
        bulletManaged.SetAngle(degreeLocal, degreeParent);
        bulletManaged.SetNewTrajectory(WeaponData.ProjectileTrajectory.normal);
    }

    public void SendEntityCollisionNotification()
    {
        bulletManaged.SetAngle(0, Random.Range(0, 361));
        bulletManaged.SetNewTrajectory(WeaponData.ProjectileTrajectory.normal);
    }
}

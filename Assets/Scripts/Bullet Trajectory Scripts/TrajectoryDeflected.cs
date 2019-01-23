using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryDeflected : MonoBehaviour, ITrajectory {

    private float degreeLocal;
    private float degreeParent;
    private float targetAngle;
    private float deflecterStrenght;
    private Vector3 deflectedFrom;

    private BulletBehaviour bulletManaged;


    public void ResetTrajectory(BulletBehaviour bulletToManage, float local, float parent)
    {
        enabled = true;
        bulletManaged = bulletToManage;
        degreeLocal = local;
        degreeParent = parent;
        if (bulletToManage.GetLastDeflecterTouched() != null)
        {
            deflectedFrom = bulletToManage.GetLastDeflecterTouched().gameObject.transform.position;
            deflecterStrenght = bulletToManage.GetLastDeflecterTouched().GetDeflectStrenght();
            degreeParent += degreeLocal;
            degreeLocal = 0;
        }
        else
        {
            bulletToManage.SetNewTrajectory(WeaponData.ProjectileTrajectory.normal);
        }
    }


    void FixedUpdate()
    {
        targetAngle = TrackTo(deflectedFrom) - 180;
        degreeParent = Mathf.MoveTowardsAngle(degreeParent, targetAngle, Time.fixedDeltaTime * bulletManaged.GetTimeScale() * deflecterStrenght);
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
        degreeLocal = 0;
        degreeParent = Random.Range(0, 361);
        bulletManaged.SetNewTrajectory(WeaponData.ProjectileTrajectory.normal);
    }
    float TrackTo(Vector3 targPos)
    {
        if (targPos == null)
            return 0;

        float difX = targPos.x - transform.position.x;
        float difY = targPos.y - transform.position.y;
        if (difX == 0)
        {
            if (difY > 0) { return 90; }
            else { return 270; }
        }

        if (difX > 0) { return Mathf.Atan(difY / difX) * Mathf.Rad2Deg; }
        else { return (Mathf.Atan(difY / difX) * Mathf.Rad2Deg) + 180; }
    }
}

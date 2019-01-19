using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryTracking : MonoBehaviour, ITrajectory
{

    private float degreeLocal;
    private float degreeParent;
    private BulletBehaviour bulletManaged;

    private IEntity target;
    private float targetAngle;

    private const float TRACKING_TURNRATE = 300;

    public void ResetTrajectory(BulletBehaviour bulletToManage, float local, float parent)
    {
        target = null;
        enabled = true;
        bulletManaged = bulletToManage;
        degreeLocal = 0;
        degreeParent = parent + local;
    }


    void FixedUpdate()
    {
        if (target == null || !target.IsAlive()) {
            target = StageManager.currentInstance.GetTargetForBullet(bulletManaged.IsAlly());
        } else if (bulletManaged.GetLifetime() > 0.25f) {
            targetAngle = TrackTo(target.GetGameObject().transform);
            degreeParent = Mathf.MoveTowardsAngle(degreeParent, targetAngle, Time.fixedDeltaTime * bulletManaged.GetTimeScale() * TRACKING_TURNRATE);
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
    }

    public void SendEntityCollisionNotification()
    {
        degreeParent = Random.Range(0, 361);
    }
    float TrackTo(Transform targTransform)
    {
        if (targTransform == null)
            return 0;

        float difX = targTransform.position.x - transform.position.x;
        float difY = targTransform.position.y - transform.position.y;
        if (difX == 0)
        {
            if (difY > 0) { return 90; }
            else { return 270; }
        }

        if (difX > 0) { return Mathf.Atan(difY / difX) * Mathf.Rad2Deg; }
        else { return (Mathf.Atan(difY / difX) * Mathf.Rad2Deg) + 180; }
    }
}

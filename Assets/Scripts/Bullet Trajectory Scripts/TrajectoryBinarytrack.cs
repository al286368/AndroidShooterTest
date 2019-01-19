using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryBinarytrack : MonoBehaviour, ITrajectory {

    private float degreeLocal;
    private float degreeParent;

    private BulletBehaviour bulletManaged;

    private bool up;
    private float offsetTime;
    private int offsetlr;
    private IEntity target;
    private const float LOCAL_ANGLE_TO_OFFSET = 300f;
    private float targetX = 0;
    private float maxXToBreakTrack = 0;
    private float minXToBreakTrack = 0;
    private int phase = 0;

    public void ResetTrajectory(BulletBehaviour bulletToManage, float local, float parent)
    {
        enabled = true;
        bulletManaged = bulletToManage;

        up = Mathf.Abs(Mathf.DeltaAngle(parent, 90)) <= 90;

        if (local == 0)
            offsetlr = 0;
        else if (local > 0)
            offsetlr = 1;
        else
            offsetlr = -1;

        offsetTime = Mathf.Abs(local / LOCAL_ANGLE_TO_OFFSET);

        targetX = 0;
        maxXToBreakTrack = 0;
        minXToBreakTrack = 0;
        degreeLocal = 0;
        phase = 0;


    }
    private void FixedUpdate()
    {
        if (offsetTime > 0)
        {
            offsetTime -= Time.fixedDeltaTime * bulletManaged.GetTimeScale();
            if (offsetlr == 0)
            {
                degreeParent = up ? 90 : 270;
            }
            else if (offsetlr < 0)
            {
                degreeParent = up ? 135 : 315;
            }
            else
            {
                degreeParent = up ? 45 : 225;
            }
        }
        else if (offsetTime > -0.2f) {
            degreeParent = up ? 90 : 270;
            offsetTime -= Time.fixedDeltaTime * bulletManaged.GetTimeScale();
        } else {
            if (phase == 0)
            {
                target = StageManager.currentInstance.GetLowestDeltaXTargetForBullet(bulletManaged.IsAlly(), transform.position.x);
                if (target != null)
                {
                    phase = 1;
                    targetX = target.GetGameObject().transform.position.x;
                    maxXToBreakTrack = Mathf.Max(transform.position.x, target.GetGameObject().transform.position.x);
                    minXToBreakTrack = Mathf.Min(transform.position.x, target.GetGameObject().transform.position.x);

                    if (transform.position.x < targetX)
                    {
                        degreeParent = up ? 45 : 315;
                    }
                    else if (transform.position.x > targetX)
                    {
                        degreeParent = up ? 135 : 225;
                    }
                    else
                    {
                        degreeParent = up ? 90 : 270;
                    }

                }
                else
                {
                    phase = 2;
                }
            }
            else if (phase == 1)
            {
                if (transform.position.x < targetX)
                {
                    degreeParent = up ? 45 : 315;
                }
                else
                {
                    degreeParent = up ? 135 : 225;
                }
                if (transform.position.x > maxXToBreakTrack || transform.position.x < minXToBreakTrack)
                {
                    phase = 2;
                    degreeParent = up ? 90 : 270;
                }
            }
            else
            {
                degreeParent = up ? 90 : 270;
            }

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

    //public void SendWallCollisionNotification(float normalAngle)
    //{
    //    offsetTime = 0.1f;
    //    if (up)
    //        offsetlr = degreeParent < 90 ? -1 : 1;
    //    else
    //        offsetlr = degreeParent < 270 ? -1 : 1;
    //}

    public void SendEntityCollisionNotification()
    {
        bulletManaged.SetAngle(0, Random.Range(0, 361));
        bulletManaged.SetNewTrajectory(WeaponData.ProjectileTrajectory.normal);
    }
}

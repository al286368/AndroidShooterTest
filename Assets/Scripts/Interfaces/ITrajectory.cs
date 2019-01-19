using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrajectory {
    void ResetTrajectory(BulletBehaviour bulletToManage, float local, float parent);
    void SendWallCollisionNotification(float normalAngle);
    void SendEntityCollisionNotification();
}

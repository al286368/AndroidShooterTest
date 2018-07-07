using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBehaviour : MonoBehaviour {

    public int bounces;
    public float beamWidth;
    public Transform visualParent;
    public Transform startPoint;
    public Transform endPoint;
    public Transform aimPoint;

    public float aimDegree = 20;

    private const float RAYCAST_REACH = 100f;
    private const float RAYCAST_BOUNCE_LIMIT_VERTICAL_ABS = 12.5f;

    public void SetShape(Vector2 basePos, int inhBounces, float degree, float width)
    {
        gameObject.SetActive(true);
        StartCoroutine("BeamAnimationNoWarning");

        transform.position = basePos;
        bounces = inhBounces;
        aimDegree = degree;
        beamWidth = width;

        aimPoint.rotation = Quaternion.Euler(-aimDegree, 90, 0);
        RaycastHit2D RH2D = Physics2D.Raycast(startPoint.position, aimPoint.forward, RAYCAST_REACH, LayerMask.GetMask("WeaponRaycasting"));
        endPoint.position = RH2D.point;

        startPoint.localScale = new Vector3(Vector3.Distance(startPoint.position,endPoint.position),beamWidth,1);
        startPoint.rotation = Quaternion.Euler(0,0,TrackTo(endPoint));
        CheckBounces();
    }
    private void CheckBounces()
    {
        if (Mathf.Abs(endPoint.position.y) > RAYCAST_BOUNCE_LIMIT_VERTICAL_ABS)
            return;
        if (!(bounces > 0))
            return;

        BeamBehaviour bounceBeam = ObjectPool.currentInstance.GetBeamFromPool();
        endPoint.position = Vector3.MoveTowards(endPoint.position, startPoint.position, 0.1f);

        bounceBeam.SetShape(endPoint.position, bounces-1, 180-aimDegree, beamWidth);

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
    IEnumerator BeamAnimationNoWarning()
    {
        float animSpeed = 4;
        float t = 1;
        visualParent.gameObject.SetActive(true);
        SpriteRenderer SR = visualParent.GetComponent<SpriteRenderer>();
        while (t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime * animSpeed);
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, t);
            visualParent.transform.localScale = new Vector3(1,(1-t)*2, 1);
            yield return null;
        }

       
    }

}

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

    private EntityBase entityUser;

    private float damage_phys = 0;
    private float damage_photon = 0;
    private float damage_cryo = 0;
    private float damage_electric = 0;
    private float damage_nuclear = 0;

    public float aimDegree = 20;

    private const float RAYCAST_REACH = 100f;
    private const float RAYCAST_BOUNCE_LIMIT_VERTICAL_ABS = 12.5f;

    private string lastCollisionTag = "";
    private Collider2D lastHitCollider;

    public void SetBeam(EntityBase e_user, Vector2 basePos, int inhBounces, float degree, Collider2D colliderToIgnore = null)
    {
        gameObject.SetActive(true);

        if (colliderToIgnore != null)
            colliderToIgnore.enabled = false;

        entityUser = e_user;

        damage_phys = e_user.stat_damage_physical;
        damage_photon = e_user.stat_damage_photon;
        damage_cryo = e_user.stat_damage_cryo;
        damage_electric = e_user.stat_damage_electric;
        damage_nuclear = e_user.stat_damage_nuclear;

        bounces = inhBounces;
        aimDegree = degree;
        beamWidth = 0.1f;

        transform.position = basePos;

        aimPoint.rotation = Quaternion.Euler(-aimDegree, 90, 0);
        RaycastHit2D RH2D = Physics2D.Raycast(startPoint.position, aimPoint.forward, RAYCAST_REACH);
        endPoint.position = RH2D.point;

        startPoint.localScale = new Vector3(Vector3.Distance(startPoint.position, endPoint.position), beamWidth, 1);
        startPoint.rotation = Quaternion.Euler(0, 0, TrackTo(endPoint));

        lastHitCollider = RH2D.collider;

        if (colliderToIgnore != null)
            colliderToIgnore.enabled = true;

        if (RH2D.collider != null)
            lastCollisionTag = RH2D.collider.gameObject.tag;
        else
            lastCollisionTag = "";

        StartCoroutine("BeamAnimationNoWarning");

        if (lastCollisionTag == "Entity")
        {
            EntityBase lastDamagedEntity = RH2D.collider.gameObject.GetComponent<EntityBase>();
            if (lastDamagedEntity != null)
            {
                HitEnemyEntity(lastDamagedEntity);
            }
        }
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
    private void HitEnemyEntity(EntityBase e)
    {
        if (entityUser.isAlly == e.isAlly)
            return;

        e.DealDamage(damage_phys, Enums.DamageType.normal);
        e.DealDamage(damage_photon, Enums.DamageType.photon);
        e.DealDamage(damage_electric, Enums.DamageType.electric);
        e.DealDamage(damage_cryo, Enums.DamageType.cryo);
        e.DealDamage(damage_nuclear, Enums.DamageType.cryo);
    }
    private void CreateNextBounceBeam()
    {
        if (Mathf.Abs(endPoint.position.y) > RAYCAST_BOUNCE_LIMIT_VERTICAL_ABS)
            return;
        if (bounces <= 0)
            return;

        switch (lastCollisionTag)
        {
            case "Border":
                {
                    BeamBehaviour bounceBeam = ObjectPool.currentInstance.GetBeamFromPool();
                    bounceBeam.SetBeam(entityUser, endPoint.position, bounces - 1, 180 - aimDegree, lastHitCollider);
                    break;
                }
            case "Entity":
                {
                    BeamBehaviour bounceBeam = ObjectPool.currentInstance.GetBeamFromPool();
                    bounceBeam.SetBeam(entityUser, endPoint.position, bounces - 1, Random.Range(0, 361), lastHitCollider);

                    break;
                }
            default:
                {
                    //BeamBehaviour bounceBeam = ObjectPool.currentInstance.GetBeamFromPool();
                    //bounceBeam.SetBeam(entityUser, endPoint.position, bounces - 1, 180 - aimDegree);
                    //Nada demomento.
                    break;
                }
        }
    }
    IEnumerator BeamAnimationNoWarning()
    {
        float animSpeed = 4;
        float t = 1;
        visualParent.gameObject.SetActive(true);
        SpriteRenderer SR = visualParent.GetComponent<SpriteRenderer>();
        while (t > 0.75f)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime * animSpeed);
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, t);
            visualParent.transform.localScale = new Vector3(1,(1-t)*2, 1);
            yield return null;
        }
        CreateNextBounceBeam();
        while (t > 0)
        {
            t = Mathf.MoveTowards(t, 0, Time.deltaTime * animSpeed);
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, t);
            visualParent.transform.localScale = new Vector3(1, (1 - t) * 2, 1);
            yield return null;
        }


    }

}

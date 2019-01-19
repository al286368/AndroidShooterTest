using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    private float speed = 10f;
    private Touch t;

    public PlayerEntity pe;

    private bool holdingFireButton = false;

    private const float BOUNDS_MAX_Y = 0;
    private const float BOUNDS_MIN_Y = -8.5f;
    private const float BOUNDS_X = 5;
    private const float OUT_OF_BOUNDS_THS = 0.25f;

    private Vector3 targetPosition;

    public Transform debug_target_pos_indicator;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ControlsComputer();
        ControlsPhone();
        CheckBounds();
    }
    public void FireButtonUp()
    {
        holdingFireButton = false;
    }
    public void FireButtonDown()
    {
        holdingFireButton = true;
    }
    private void ControlsComputer()
    {
        if (holdingFireButton)
            pe.Shoot(90);
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.fixedDeltaTime * speed);
    }
    private void ControlsPhone()
    {
        if (holdingFireButton)
            pe.Shoot(90);
        if (Input.touchCount == 0)
            return;
        for (int i = 0; i < Input.touchCount; i++)
        {
            t = Input.GetTouch(i);
            if (t.phase != TouchPhase.Began && IsPointerBelowHalfScreen(t) )
            {
                targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, 0);
                debug_target_pos_indicator.transform.position = targetPosition;
            }
               
        }

        //targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, 0);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.fixedDeltaTime * speed);
    }
    private bool IsPointerBelowHalfScreen(Touch t)
    {
        return t.position.y < Screen.height / 2;
    }
    private void CheckBounds()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -BOUNDS_X, BOUNDS_X), Mathf.Clamp(transform.position.y, BOUNDS_MIN_Y, BOUNDS_MAX_Y), 0);
    }
}

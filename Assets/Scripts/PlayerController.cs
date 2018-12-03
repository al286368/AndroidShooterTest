﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    private float speed = 10f;
    private Touch t;

    public EntityBase EB;

    private bool holdingFireButton = false;

    private const float BOUNDS_MAX_Y = 0;
    private const float BOUNDS_MIN_Y = -6.5f;
    private const float OUT_OF_BOUNDS_THS = 0.25f;

    private Vector3 targetPosition;

    private float tmp_dist_to_bounds_x_min;
    private float tmp_dist_to_bounds_x_max;

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
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.fixedDeltaTime * speed);
    }
    private void ControlsPhone()
    {
        if (holdingFireButton)
            EB.Shoot();
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
        tmp_dist_to_bounds_x_max = transform.position.x - StageManager.currentInstance.BOUNDS_MAX_X;
        tmp_dist_to_bounds_x_min = transform.position.x - StageManager.currentInstance.BOUNDS_MIN_X;
        if (tmp_dist_to_bounds_x_max > OUT_OF_BOUNDS_THS)
        {
            PlayerOutOfBounds();
        }
        if (tmp_dist_to_bounds_x_min < OUT_OF_BOUNDS_THS)
        {
            PlayerOutOfBounds();
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, StageManager.currentInstance.BOUNDS_MIN_X, StageManager.currentInstance.BOUNDS_MAX_X), Mathf.Clamp(transform.position.y, BOUNDS_MIN_Y, BOUNDS_MAX_Y), 0);
    }
    private void PlayerOutOfBounds()
    {
        
    }
}
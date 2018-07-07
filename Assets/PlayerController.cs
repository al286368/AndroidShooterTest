using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float speed = 10f;
    private Touch t;

    private const float BOUNDS_MAX_Y = 0;
    private const float BOUNDS_MIN_Y = -6.5f;
    private const float OUT_OF_BOUNDS_THS = 0.25f;

    private Vector3 targetPosition;

    private float tmp_dist_to_bounds_x_min;
    private float tmp_dist_to_bounds_x_max;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControlsComputer();
        CheckBounds();
    }
    private void ControlsComputer()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.fixedDeltaTime * speed);
    }
    private void ControlsPhone()
    {
        t = Input.GetTouch(0);
        targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, 0);
        targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.fixedDeltaTime * speed);
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

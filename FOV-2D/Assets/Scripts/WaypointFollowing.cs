using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollowing : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed = 0.3f;
    [SerializeField] float flipRotationSpeed = 10f;
    private int cur;

    private void Start()
    {
        this.transform.position = waypoints[0].transform.position;
        ResetCur();
    }

    void Update()
    {
        Movement();

        Flip();
    }

    private void Flip()
    {
        Vector2 dir = waypoints[cur].position - transform.position;

        if (dir.x > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.AngleAxis(0, Vector3.forward), flipRotationSpeed * Time.deltaTime);
        }
        if (dir.x < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.AngleAxis(180, Vector3.forward), flipRotationSpeed * Time.deltaTime);
        }
        if (dir.y > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.AngleAxis(90, Vector3.forward), flipRotationSpeed * Time.deltaTime);
        }
        if (dir.y < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.AngleAxis(-90, Vector3.forward), flipRotationSpeed * Time.deltaTime);
        }
    }

    private void Movement()
    {
        // Waypoint not reached yet? then move closer
        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else
        {
            cur = (cur + 1) % waypoints.Length;
        }
    }

    public void ResetCur()
    {
        cur = 0;
    }
}

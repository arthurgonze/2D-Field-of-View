using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollowMouse : MonoBehaviour
{
    Camera viewCamera;
    float angle;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        viewCamera = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = viewCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.AngleAxis(angle, Vector3.forward), /*SPEED*/10000 * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVEnemyRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);
    }
}

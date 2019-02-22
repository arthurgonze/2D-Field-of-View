using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            this.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y + walkSpeed * Time.deltaTime,
                0);
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            this.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y - walkSpeed * Time.deltaTime,
                0);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            this.transform.position = new Vector3(this.transform.position.x + walkSpeed * Time.deltaTime,
                this.transform.position.y,
                0);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            this.transform.position = new Vector3(this.transform.position.x - walkSpeed * Time.deltaTime,
                this.transform.position.y,
                0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour {

    public float cameraMoveSpeed;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update ()
    {
        // Mouse movement
        if (Input.mousePosition.y <= 0)
        {
            transform.position -= transform.forward * cameraMoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height)
        {
            transform.position += transform.forward * cameraMoveSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x <= 0)
        {
            transform.Translate (Vector3.left * cameraMoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.mousePosition.x >= Screen.width)
        {
            transform.Translate (Vector3.right * cameraMoveSpeed * Time.deltaTime, Space.Self);
        }

        //Keyboard movement
        transform.position += (transform.forward * cameraMoveSpeed * Time.deltaTime) * Input.GetAxis ("Vertical");
        transform.Translate ((new Vector3 (Input.GetAxis ("Horizontal"), 0, 0) * cameraMoveSpeed * Time.deltaTime), Space.Self);
    }
}

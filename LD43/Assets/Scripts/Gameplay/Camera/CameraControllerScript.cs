using System;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour {

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    public float m_CameraMoveSpeed;
    private float m_ScreenBorderMargin = 30;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update ()
    {
        // Mouse movement
        if (Input.mousePosition.y <= 20 && Input.mousePosition.y >= 0)
        {
            transform.Translate (Vector3.down * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.mousePosition.y <= Screen.height && Input.mousePosition.y >= Screen.height - m_ScreenBorderMargin)
        {
            transform.Translate (Vector3.up * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.mousePosition.x <= 20 && Input.mousePosition.x >= 0)
        {
            transform.Translate (Vector3.left * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.mousePosition.x <= Screen.width && Input.mousePosition.x >= Screen.width - m_ScreenBorderMargin)
        {
            transform.Translate (Vector3.right * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
        }

        //Keyboard movement
        transform.Translate ((new Vector3 (0, Input.GetAxis ("Vertical"), 0) * m_CameraMoveSpeed * Time.deltaTime), Space.Self);
        transform.Translate ((new Vector3 (Input.GetAxis ("Horizontal"), 0, 0) * m_CameraMoveSpeed * Time.deltaTime), Space.Self);
    }

    private void LateUpdate ()
    {
        var v3 = transform.position;
        v3.x = Mathf.Clamp (v3.x, minX, maxX);
        v3.y = Mathf.Clamp (v3.y, minY, maxY);
        transform.position = v3;
    }
}

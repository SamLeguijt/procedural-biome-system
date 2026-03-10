using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float mouseSensitivity = 2f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse look
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");  

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.Q))
            move += Vector3.down;

        if (Input.GetKey(KeyCode.E))
            move += Vector3.up;

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}

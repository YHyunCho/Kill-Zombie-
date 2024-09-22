using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;
    public MainManager mainManager;
    public Camera firstViewCam;

    public LayerMask cameraCollision;
    Vector3 offset = new Vector3(0, 1.4f, -2.4f);

    private float mouseXInput = 0;
    private float mouseYInput = 0;
    private float xRotation = 0;
    private float yRotation = 0;
    private float speed = 1;

    public float range = 2;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        if (mainManager.isGameActive)
        {
            transform.position = player.transform.position + transform.rotation * offset;

            Quaternion currentRotation = Quaternion.Euler(xRotation, yRotation, 0);

            mouseXInput = Input.GetAxis("Mouse X");
            mouseYInput = Input.GetAxis("Mouse Y");

            xRotation -= mouseYInput;
            yRotation += mouseXInput;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            Quaternion destination = Quaternion.Euler(xRotation, yRotation, 0);

            transform.rotation = Quaternion.Slerp(currentRotation, destination, speed);
            CheckCollision();
        }
    }

    private void CheckCollision()
    {
        Vector3 rayDir = transform.position - player.transform.position;

        if (Physics.Raycast(player.transform.position, rayDir, out RaycastHit hitBack, range, cameraCollision))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, hitBack.point.z - rayDir.normalized.z);
        } 
    }

    public void SwitchToThirdPerson()
    {
        mouseXInput = firstViewCam.transform.rotation.eulerAngles.y;
        mouseYInput = firstViewCam.transform.rotation.eulerAngles.x;

        transform.rotation = firstViewCam.transform.rotation;
    }
}

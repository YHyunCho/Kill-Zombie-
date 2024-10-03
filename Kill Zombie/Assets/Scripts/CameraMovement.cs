using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public MainManager mainManager;

    Vector3 offset;

    protected float mouseXInput = 0;
    protected float mouseYInput = 0;
    private float xRotation = 0;
    private float yRotation = 0;
    private float speed = 1;

    public float range = 2;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    protected void CameraRotation()
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
        }
    }

    public void SwitchTo(Camera camera)
    {
        mouseXInput = camera.transform.rotation.eulerAngles.y;
        mouseYInput = camera.transform.rotation.eulerAngles.x;

        transform.rotation = camera.transform.rotation;
    }
}

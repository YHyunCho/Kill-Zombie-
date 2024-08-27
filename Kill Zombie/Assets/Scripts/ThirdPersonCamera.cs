using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;
    public Camera firstViewCam;

    private float mouseXInput = 0;
    private float mouseYInput = 0;

    private Vector3 thirdPersonOffset = new Vector3(0, -1.4f, 2.4f);

    private void LateUpdate()
    {
        mouseXInput += Input.GetAxis("Mouse X");
        mouseYInput -= Input.GetAxis("Mouse Y");

        transform.rotation = Quaternion.Euler(mouseYInput, mouseXInput, 0);
        transform.position = player.transform.position - transform.rotation * thirdPersonOffset;
    }

    private void CheckCollision()
    {
        Vector3 rayDirection;
    }

    public void SwitchToThirdPerson()
    {
        mouseXInput = firstViewCam.transform.rotation.eulerAngles.y;
        mouseYInput = firstViewCam.transform.rotation.eulerAngles.x;

        transform.rotation = transform.rotation;
    }
}

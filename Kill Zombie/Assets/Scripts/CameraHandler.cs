using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject player;

    public Camera firstViewCam;
    public Camera thirdViewCam;
    public Camera deathViewCam;

    public Canvas crosshair;

    private float mouseXInput = 0;
    private float mouseYInput = 0;

    private Vector3 thirdPersonOffset = new Vector3(0, -1.4f, 2.4f);
    private Vector3 deathCamOffset = new Vector3(0, 2.3f, 1.5f);

    void Start()
    {
        ActivateThirdPersonCamera();

        deathViewCam.enabled = false;
    }

    public void ActivateFirstPersonCamera()
    {
        thirdViewCam.enabled = false;
        firstViewCam.enabled = true;

        crosshair.enabled = true;
    }

    public void ActivateThirdPersonCamera()
    {
        thirdViewCam.enabled = true;
        firstViewCam.enabled = false;

        crosshair.enabled = false;

        ThirdPersonCameraMovement();
    }

    public void ActivateDeathCamera()
    {
        thirdViewCam.enabled = false;
        firstViewCam.enabled = true;
        deathViewCam.enabled = false;

        crosshair.enabled = false;

        //DeathViewCamTransform();
    }

    void ThirdPersonCameraMovement()
    {
        mouseXInput += Input.GetAxis("Mouse X");
        mouseYInput -= Input.GetAxis("Mouse Y");

        thirdViewCam.transform.rotation = Quaternion.Euler(mouseYInput, mouseXInput, 0);
        thirdViewCam.transform.position = player.transform.position - thirdViewCam.transform.rotation * thirdPersonOffset;
    }

    public void SwitchToThirdPerson()
    {
        mouseXInput = firstViewCam.transform.rotation.eulerAngles.y;
        mouseYInput = firstViewCam.transform.rotation.eulerAngles.x;

        thirdViewCam.transform.rotation = firstViewCam.transform.rotation;
    }

    void DeathViewCamTransform()
    {
        transform.position = player.transform.position + deathCamOffset;
        transform.rotation = Quaternion.Euler(45, 180, 0);
    }
}

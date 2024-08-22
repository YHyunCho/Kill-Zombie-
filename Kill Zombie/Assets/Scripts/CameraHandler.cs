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

    private Vector3 mainCamOffset = new Vector3(0, -1.4f, 2.4f);
    private Vector3 deathCamOffset = new Vector3(0, 2.3f, 1.5f);

    void Start()
    {
        ThirdViewCameraOn();

        deathViewCam.enabled = false;
    }

    public void FirstViewCameraOn()
    {
        thirdViewCam.enabled = false;
        firstViewCam.enabled = true;

        crosshair.enabled = true;
    }

    public void ThirdViewCameraOn()
    {
        thirdViewCam.enabled = true;
        firstViewCam.enabled = false;

        crosshair.enabled = false;

        CameraMove(thirdViewCam, mainCamOffset);
    }

    public void DeathViewCameraOn()
    {
        thirdViewCam.enabled = false;
        firstViewCam.enabled = false;
        deathViewCam.enabled = true;

        crosshair.enabled = false;

        DeathViewCamTransform();
    }

    void CameraMove(Camera camera, Vector3 offset)
    {
        mouseXInput += Input.GetAxis("Mouse X");
        mouseYInput -= Input.GetAxis("Mouse Y");

        camera.transform.rotation = Quaternion.Euler(mouseYInput, mouseXInput, 0);
        camera.transform.position = player.transform.position - camera.transform.rotation * offset;
    }

    public void AfterMouseUp(Quaternion firstViewCamRotation)
    {
        mouseXInput = firstViewCamRotation.eulerAngles.y;
        mouseYInput = firstViewCamRotation.eulerAngles.x;

        thirdViewCam.transform.rotation = firstViewCamRotation;
    }

    void DeathViewCamTransform()
    {
        transform.position = player.transform.position + deathCamOffset;
        transform.rotation = Quaternion.Euler(45, 180, 0);
    }
}

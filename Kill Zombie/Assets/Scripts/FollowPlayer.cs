using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private SwichCamera swichCameraScript;

    private float mouseXInput = 0;
    private float mouseYInput = 0;

    public float turnSpeed;
    private Vector3 mainCamOffset = new Vector3(0, -1.4f, 2.4f);
    private Vector3 deathCamOffset = new Vector3(0, 2.3f, 10f);

    void Start()
    {
        swichCameraScript = GameObject.Find("CameraManager").GetComponent<SwichCamera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (swichCameraScript.thirdView.enabled == true)
        {
            FirstViewCamera();

        } else if (swichCameraScript.deathView.enabled == true)
        {
            DeathViewCamera();
        }

        //mouseXInput += Input.GetAxis("Mouse X");
        //mouseYInput -= Input.GetAxis("Mouse Y");
        //transform.rotation = Quaternion.Euler(mouseYInput, mouseXInput, 0);

        //transform.position = player.transform.position - transform.rotation * offset;

    }

    void FirstViewCamera()
    {
        mouseXInput += Input.GetAxis("Mouse X");
        mouseYInput -= Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(mouseYInput, mouseXInput, 0);

        transform.position = player.transform.position - transform.rotation * mainCamOffset;
    }

    void DeathViewCamera()
    {
        transform.position = player.transform.position + deathCamOffset;
        transform.rotation = Quaternion.Euler(40, 180, 0);
    }

}

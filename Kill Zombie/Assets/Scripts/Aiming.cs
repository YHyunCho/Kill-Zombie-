using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public GameObject player;
    public GameObject thirdViewCam;

    public float mouseXInput;
    public float mouseYInput;

    private Vector3 offset = new Vector3(0.017f, -1.1f, -0.12f);

    void LateUpdate()
    {
        mouseXInput += Input.GetAxis("Mouse X");
        mouseYInput -= Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(mouseYInput, mouseXInput, 0);

        transform.position = player.transform.position - transform.rotation * offset;

        Vector3 thirdToFirst = thirdViewCam.transform.forward;
        thirdToFirst.y = 0;
        transform.LookAt(transform.position + thirdToFirst);
    }
}

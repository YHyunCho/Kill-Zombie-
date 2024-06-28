using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public GameObject player;
    public GameObject thirdViewCam;

    private float turnSpeed = 400;
    private Vector3 offset = new Vector3(0.017f, -1.1f, -0.12f);

    private void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position - transform.rotation * offset;

        Vector3 thirdToFirst = thirdViewCam.transform.forward;
        thirdToFirst.y = 0;
        transform.LookAt(transform.position + thirdToFirst);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    private float mouseXInput = 0;
    private float mouseYInput = 0;

    public float turnSpeed;
    private Vector3 offset = new Vector3(0, -1.4f, 2.4f);

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        mouseXInput += Input.GetAxis("Mouse X");
        mouseYInput -= Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(mouseYInput, mouseXInput, 0);

        transform.position = player.transform.position - transform.rotation * offset;

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;
    public Camera firstViewCam;

    public LayerMask cameraCollision;
    private Vector3 thirdPersonOffset = new Vector3(0, -1.7f, 1.5f);

    private float mouseXInput = 0;
    private float mouseYInput = 0;
    private float xRotation = 0;
    private float yRotation = 0;
    [SerializeField] private float speed = 0;

    public float range = 2;

    private void LateUpdate()
    {
        Quaternion currentRotation = Quaternion.Euler(xRotation, yRotation, 0);

        mouseXInput = Input.GetAxis("Mouse X");
        mouseYInput = Input.GetAxis("Mouse Y");

        xRotation -= mouseYInput;
        yRotation += mouseXInput;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Quaternion destination = Quaternion.Euler(xRotation, yRotation, 0);


        transform.rotation = Quaternion.Slerp(currentRotation, destination, 1);
        CheckCollision();
        //transform.position = player.transform.position - transform.rotation * thirdPersonOffset;
    }

    private void OnDrawGizmos()
    {
        //Vector3 rayDir = player.transform.position - transform.position;
        Vector3 rayDir = transform.position - player.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(player.transform.position, rayDir * range);
        //Gizmos.DrawRay(transform.position, -rayDir * range);
    }

    private void CheckCollision()
    {
        Vector3 rayDir = transform.position - player.transform.position;

        //if (Physics.Raycast(transform.position, -rayDir, out RaycastHit hitForward, range, cameraCollision))
        //{
        //    //transform.position = hitForward.point - rayDir.normalized;
        //    transform.Translate((transform.position - hitForward.point) * Time.deltaTime * speed, Space.World);
        //}
        if (Physics.Raycast(player.transform.position, rayDir, out RaycastHit hitBack, range, cameraCollision))
        {
            Debug.Log("Ray with " + hitBack.collider.name);

            transform.position = hitBack.point - rayDir.normalized;
            //transform.position.x = player.transform.position.x;
            //transform.Translate((hitBack.point - rayDir.normalized) * Time.deltaTime * speed, Space.World);

        } 
    }

    public void SwitchToThirdPerson()
    {
        mouseXInput = firstViewCam.transform.rotation.eulerAngles.y;
        mouseYInput = firstViewCam.transform.rotation.eulerAngles.x;

        transform.rotation = transform.rotation;
    }
}

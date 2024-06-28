using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private SwichCamera swichCameraScript;

    public GameObject thirdViewCam;
    public GameObject firstViewCam;
    public GameObject bulletPrefab;

    private Quaternion initialRotation;
    private Vector3 shootOffset = new Vector3(0, 1, 0);

    private float stairForce = 120;
    private float jumpForce = 550;
    private float gravityModifer = 2.5f;
    private float speed = 250000;
    private float turnSpeed = 50;

    private bool isOnGround = true;
    private bool isOnStair;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        swichCameraScript = GameObject.Find("CameraManager").GetComponent<SwichCamera>();

        Physics.gravity *= gravityModifer;
    }

    // Update is called once per frame
    void Update()
    {
        initialRotation = transform.rotation;
        Vector2 inputVector = new Vector2(0, 0);

        /* 
         Set up movement
         WASD - forward/back/left/right
         Space - Jump
         Left Mouse Button - shoot
        */
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (!isOnStair)
            {
                inputVector.y = 1f;
            }
            else if(isOnStair)
            {
                playerRb.AddForce(Vector3.up * stairForce, ForceMode.Impulse);
            }
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1f;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // Prevent double-jumping
            isOnGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bulletPrefab, transform.position + shootOffset, transform.rotation);
        }

        CameraSight(thirdViewCam);
        CameraSight(firstViewCam);

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        inputVector = inputVector.normalized;

        if (!(inputVector.x == 0 && inputVector.y == 0))
        {
            //    moveDir = transform.TransformDirection(moveDir);
            //    playerRb.AddForce(moveDir * speed * Time.deltaTime);
            //    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turnSpeed);

            if (swichCameraScript.thirdView.enabled == true)
            {
                moveDir = transform.TransformDirection(moveDir);
                playerRb.AddForce(moveDir * speed * Time.deltaTime);
                // When player presses left/right/back key, object also rotates same direction.
                transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turnSpeed);
            }
            else
            {
                playerRb.AddForce(moveDir * speed * Time.deltaTime);
            }

        playerAnim.SetBool("Run_bool", true);

        } else
        {
            playerAnim.SetBool("Run_bool", false);
            transform.rotation = initialRotation;
        }

    }

    void CameraSight(GameObject camera)
    {
        // When Camera moves, objects move to same direction.
        Vector3 offset = camera.transform.forward;
        offset.y = 0;
        transform.LookAt(transform.position + offset);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isOnStair = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject && !collision.gameObject.CompareTag("Stairs"))
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isOnStair = false;
        }
    }
}

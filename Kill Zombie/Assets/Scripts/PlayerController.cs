using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private SwichCamera swichCameraScript;
    private Aiming aimingScript;

    public GameObject thirdViewCam;
    public GameObject firstViewCam;
    public GameObject bulletPrefab;

    private Quaternion initialRotation;
    private Vector3 shootOffset = new Vector3(0, 1, 1);

    private float stairForce = 120;
    private float jumpForce = 550;
    private float gravityModifer = 2.5f;
    private float speed = 250000;
    private float turnSpeed = 50;

    public bool gameOver;
    private bool isOnGround = true;
    private bool isOnStair;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        swichCameraScript = GameObject.Find("CameraManager").GetComponent<SwichCamera>();
        aimingScript = GameObject.Find("FirstViewCam").GetComponent<Aiming>();

        Physics.gravity *= gravityModifer;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == false)
        {
            PlayerMovement();
        }
    }

    void PlayerMovement()
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
            else if (isOnStair)
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && swichCameraScript.firstView.enabled == true)
        {
            Instantiate(bulletPrefab, transform.position + shootOffset, transform.rotation);
        }

        CameraSight(thirdViewCam);
        CameraSight(firstViewCam);

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        inputVector = inputVector.normalized;
        moveDir = transform.TransformDirection(moveDir);

        if (swichCameraScript.thirdView.enabled == true)
        {
            ThirdViewMovement(inputVector, moveDir);
        }
        else
        {
            FirstViewMoveMent(inputVector, moveDir);
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
            isOnGround = false;

        } else if (!collision.gameObject.CompareTag("Stairs"))
        {
            isOnGround = true;
        }

        //if (collision.gameObject.CompareTag("Zombie"))
        //{
        //    gameOver = true;
        //    playerRb.velocity = Vector3.zero;
        //    playerAnim.SetBool("Run_bool", false);
        //    Debug.Log("GAME OVER");
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isOnStair = false;
        }
    }

    void ThirdViewMovement(Vector3 inputVector, Vector3 moveDir)
    {
        if (!(inputVector.x == 0 && inputVector.y == 0))
        {
            playerAnim.SetBool("Run_bool", true);

            playerRb.AddForce(moveDir * speed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turnSpeed);
        }
        else
        {
            playerAnim.SetBool("Run_bool", false);
            transform.rotation = initialRotation;
        }
    }

    void FirstViewMoveMent(Vector3 inputVector, Vector3 moveDir)
    {

        if (!(inputVector.x == 0 && inputVector.y == 0))
        {
            //playerAnim.SetBool("Run_bool", true);

            playerRb.AddForce(moveDir * speed * Time.deltaTime);
        }
        else
        {
            //playerAnim.SetBool("Run_bool", false);
            transform.rotation = initialRotation;
        }
        transform.rotation = Quaternion.Euler(0, aimingScript.mouseXInput, 0);
    }
}
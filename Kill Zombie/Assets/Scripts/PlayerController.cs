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
    private Quaternion lookForward;
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
        if (gameOver)
        {
            playerRb.velocity = Vector3.zero;
        }
        else 
        {
            initialRotation = transform.rotation;
            lookForward = firstViewCam.transform.rotation;

            Vector2 userInput = PlayerMovement();
            Vector3 moveDir = new Vector3(userInput.x, 0, userInput.y);
            moveDir = transform.TransformDirection(moveDir);

            if (swichCameraScript.thirdView.enabled == true)
            {
                ThirdViewMovement(userInput, moveDir);
            }
            else
            {
                FirstViewMovement(userInput, moveDir);
                ShootWeapon();
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isOnStair = true;
            isOnGround = false;
        }
        else if (!collision.gameObject.CompareTag("Stairs"))
        {
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("Zombie"))
        {
            gameOver = true;
            playerAnim.SetBool("Run_bool", false);
            Debug.Log("GAME OVER");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isOnStair = false;
        }
    }

    Vector2 PlayerMovement()
    {
        Vector2 inputVector = new Vector2(0, 0);

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
        inputVector = inputVector.normalized;

        FaceCameraDirection(thirdViewCam);
        FaceCameraDirection(firstViewCam);

        return inputVector;
    }

    void FaceCameraDirection(GameObject camera)
    {
        // When Camera moves, objects move to same direction.
        Vector3 offset = camera.transform.forward;
        offset.y = 0;
        transform.LookAt(transform.position + offset);
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

    void FirstViewMovement(Vector3 inputVector, Vector3 moveDir)
    {
        if (!(inputVector.x == 0 && inputVector.y == 0))
        {
            playerRb.AddForce(moveDir * speed * Time.deltaTime);
        }
        else
        {
            transform.rotation = lookForward;
        }
        transform.rotation = Quaternion.Euler(aimingScript.mouseYInput, aimingScript.mouseXInput, 0);
    }

    void ShootWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bulletPrefab, transform.position + shootOffset, transform.rotation);
        }
    }
}
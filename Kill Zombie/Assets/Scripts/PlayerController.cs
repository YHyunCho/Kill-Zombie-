using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected CameraHandler updateView;

    protected Rigidbody playerRb;
    protected Animator playerAnim;

    public Camera thirdViewCam;
    public Camera firstViewCam;
    public GameObject bulletPrefab;

    protected Quaternion initialRotation;
    protected Quaternion lookForward;
    private Vector3 shootOffset = new Vector3(0, 1, 1);
    protected Vector3 camOffset = new Vector3(-0.12f, 0.11f, 0);

    protected float stairForce = 120;
    protected float jumpForce = 550;
    protected float gravityModifer = 2.5f;
    protected float speed = 250000;
    protected float turnSpeed = 50;
    protected float xRotation;
    protected float yRotation;
    protected float mouseX;
    protected float mouseY;

    protected bool isOnGround = true;
    protected bool isOnStair;
    private bool isFirstPerson = false;

    protected float h;
    protected float v;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        updateView = GameObject.Find("Cameras").GetComponent<CameraHandler>();
        
        playerRb.freezeRotation = true;
        Physics.gravity *= gravityModifer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameActive)
        {
            updateView.DeathViewCameraOn();
            playerRb.velocity = Vector3.zero;
        }
        else 
        {
            initialRotation = transform.rotation;
            lookForward = firstViewCam.transform.rotation;

            if (Input.GetMouseButton(1))
            {
                isFirstPerson = true;
                updateView.FirstViewCameraOn();
                FirstPersonControl();
            } else
            {
                updateView.ThirdViewCameraOn();
                if(isFirstPerson)
                {
                    isFirstPerson = false;
                    updateView.AfterMouseUp(firstViewCam.transform.rotation);
                }
                ThirdPersonControl();
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
            GameManager.Instance.isGameActive = false;
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
 
    private void FirstPersonControl()
    {
        FirstviewRotate();
        FirstViewMove();

    }

    void FirstviewRotate()
    {
        mouseX = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        firstViewCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        FaceCameraDirection(thirdViewCam);

        Vector3 thirdToFirst = transform.forward;
        thirdToFirst.y = 0;
        firstViewCam.transform.LookAt(firstViewCam.transform.position + thirdToFirst);
    }

    void FirstViewMove()
    {
        h = Input.GetAxisRaw("Horizontal"); 
        v = Input.GetAxisRaw("Vertical");   

        Vector3 moveVec = transform.forward * v + transform.right * h;

        if (!(h == 0 && v == 0))
        {
            playerAnim.SetBool("Run_bool", true);
            playerRb.AddForce(moveVec * speed * Time.deltaTime);
        }
        else
        {
            playerAnim.SetBool("Run_bool", false);
        }
    }

    private void ThirdPersonControl()
    {
        Vector2 userInput = PlayerMovement();
        Vector3 moveDir = new Vector3(userInput.x, 0, userInput.y);
        moveDir = transform.TransformDirection(moveDir);

        ThirdViewMovement(userInput, moveDir);
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

        return inputVector;
    }

    void FaceCameraDirection(Camera camera)
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

}
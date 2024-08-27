using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CameraHandler updateView;
    private ThirdPersonCamera thirdPersonCam;

    private Rigidbody playerRb;
    private Animator playerAnim;

    public Camera thirdViewCam;
    public GameObject firstViewCam;
    public Transform zombiePerfab;

    private float gravityModifer = 2.5f;

    // Camera Variables
    private bool isFirstView = false;

    // Common Variables 
    private float speed = 250000;
    private float turnSpeed = 50;

    // Third Person Variables
    private Quaternion initialRotation;
    private float stairForce = 120;
    private float jumpForce = 550;

    // First Person Variables
    private float xRotation;
    private float yRotation;
    private float mouseX;
    private float mouseY;
    private float horizontalInput;
    private float verticalInput;

    // Collision Detection Variables
    private bool isOnGround = true;
    private bool isOnStair;

    // Player Shooting Variable
    private float range = 30;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        updateView = GameObject.Find("Cameras").GetComponent<CameraHandler>();
        thirdPersonCam = GameObject.Find("ThridViewCam").GetComponent<ThirdPersonCamera>();
        
        playerRb.freezeRotation = true;
        Physics.gravity *= gravityModifer;
    }

    void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            initialRotation = transform.rotation;

            if (Input.GetMouseButton(1))
            {
                updateView.ActivateFirstPersonCamera();

                if (!isFirstView)
                {
                    isFirstView = true;
                    SwitchToFirstPerson();
                }

                FirstPersonControl();

                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                    playerAnim.SetTrigger("Shoot_trig");
                }
            }
            else
            {
                updateView.ActivateThirdPersonCamera();

                if (isFirstView)
                {
                    isFirstView = false;
                    thirdPersonCam.SwitchToThirdPerson();
                }

                ThirdPersonControl();
            }
        }
    }

    // Player Shooting

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(firstViewCam.transform.position, firstViewCam.transform.forward * range);
    }

    public void Shoot()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(firstViewCam.transform.position, firstViewCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.tag == "Zombie")
            {
                ZombieController zombie = hit.collider.GetComponent<ZombieController>();
                if(zombie.isAlive)
                {
                    zombie.OnHit();
                    GameManager.Instance.UpdateBodyCount();
                    GameManager.Instance.UpdateLevel();
                }
                zombie.isAlive = false;
            } else if(hit.collider.tag == "FireWood") 
            {
                Destroy(hit.collider.gameObject);
                GameManager.Instance.isFireWoodDestroyed = true;
                GameManager.Instance.UpdateLevel();
            }
        }
    }

    // Collision Detection

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isOnStair = true;
            isOnGround = false;
            playerAnim.SetBool("RunForward_bool", false);
        }
        else if (!collision.gameObject.CompareTag("Stairs"))
        {
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("Zombie"))
        {
            PlayerDeath(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isOnStair = false;
        }
    }

    // When player collides with alive zombie, it will dead

    private void PlayerDeath(GameObject collideZombie)
    {
        ZombieController zombie = collideZombie.GetComponent<ZombieController>();
        if (zombie.isAlive)
        {
            GameManager.Instance.isGameActive = false;
            updateView.ActivateDeathCamera(collideZombie.transform);

            zombie.AttackPlayer();
            Invoke("PlayerFall", 0.5f);

            Debug.Log("GAME OVER");
            gameObject.SetActive(false);
        }
    }

    private void PlayerFall()
    {
        updateView.CameraPlayerHitReaction();
    }
 
    // First-Person Movement

    private void FirstPersonControl()
    {
        FirstPersonRotation();
        FirstPersonMoveMent();
    }

    private void SwitchToFirstPerson()
    {
        yRotation = thirdViewCam.transform.rotation.eulerAngles.y;
    }

    void FirstPersonRotation()
    {
        mouseX = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        firstViewCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void FirstPersonMoveMent()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        verticalInput = Input.GetAxisRaw("Vertical");   

        Vector3 moveVec = transform.forward * verticalInput + transform.right * horizontalInput;

        if (!(horizontalInput == 0 && verticalInput == 0))
        {
            playerAnim.SetBool("Run_bool", true);
            if(verticalInput > 0)
            {
                playerAnim.SetBool("RunForward_bool", true);
            } else
            {
                playerAnim.SetBool("RunForward_bool", false);
            }
            playerRb.AddForce(moveVec * speed * Time.deltaTime);
        }
        else
        {
            playerAnim.SetBool("Run_bool", false);
            playerAnim.SetBool("RunForward_bool", false);
        }
    }

    // Third-Person Movement

    private void ThirdPersonControl()
    {
        Vector2 userInput = SavedInputKey();
        Vector3 moveDir = new Vector3(userInput.x, 0, userInput.y);
        moveDir = transform.TransformDirection(moveDir);

        ThirdPersonMovement(userInput, moveDir);
    }

    Vector2 SavedInputKey()
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

        AlignWithCameraForward();

        return inputVector;
    }

    void AlignWithCameraForward()
    {
        // When Camera moves, objects move to same direction.
        Vector3 offset = thirdViewCam.transform.forward;
        offset.y = 0;
        transform.LookAt(transform.position + offset);
    }

    void ThirdPersonMovement(Vector3 inputVector, Vector3 moveDir)
    {
        if (!(inputVector.x == 0 && inputVector.y == 0))
        {
            playerAnim.SetBool("Run_bool", true);
            playerAnim.SetBool("RunForward_bool", true);

            playerRb.AddForce(moveDir * speed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turnSpeed);
        }
        else
        {
            playerAnim.SetBool("Run_bool", false);
            playerAnim.SetBool("RunForward_bool", false);
            transform.rotation = initialRotation;
        }
    }
}
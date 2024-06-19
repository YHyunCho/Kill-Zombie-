using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public GameObject Cam;
    public float stairForce;

    private float jumpForce = 600;
    private float gravityModifer = 2.5f;
    private float speed = 250000;
    private float turnSpeed = 50;

    private bool isOnGround = true;
    private bool isOnStair;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifer;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);

        /* 
         Set up movement
         WASD - forward/back/left/right
         Space - Jump
        */
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1f;
        }
        if (Input.GetKey(KeyCode.W) && !isOnStair)
        {
            inputVector.y = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1f;
        }
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // Prevent double-jumping
            isOnGround = false;
        }
        
        // When Camera moves, objects move to same direction.
        Vector3 offset = Cam.transform.forward;
        offset.y = 0;
        transform.LookAt(transform.position + offset);

        // When player presses left/right/back key, object also rotates same direction.
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        inputVector = inputVector.normalized;

        if (!(inputVector.x == 0 && inputVector.y == 0))
        {
            moveDir = transform.TransformDirection(moveDir);
            playerRb.AddForce(moveDir * speed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turnSpeed);
        }
        Debug.Log(isOnStair);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("Stairs") && Input.GetKey(KeyCode.W))
        {
            isOnStair = true;
            if (isOnStair)
            {
                playerRb.AddForce(Vector3.up * stairForce * Time.deltaTime);
            }
        } else
        {
            isOnStair = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private SpawnManager spawnScript;
    public Camera shootCamera;

    public float speed;

    private float yPosRange = 6;
    private float xPosRange = 9.8f;
    private float zPosRange = 14.8f;

    private Vector3 middleOfCamera = new Vector3(0.5f, 0.5f, 0);

    void Start()
    {
        spawnScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = shootCamera.ViewportPointToRay(middleOfCamera);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000); 
        }

        Vector3 direction = targetPoint - transform.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * speed;
        DestroyOutOfBound();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Zombie"))
        {
            Destroy(gameObject);
            Debug.Log("Collision detected with: " + other.gameObject.name);
        }
        else if (other.gameObject.CompareTag("Zombie"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            spawnScript.currentZombieCount -= 1;
        }
    }

    void DestroyOutOfBound()
    {
        if (transform.position.x > xPosRange || transform.position.x < -xPosRange)
        {
            Destroy(gameObject);
        } else if (transform.position.z > zPosRange || transform.position.z < -zPosRange)
        {
            Destroy(gameObject);
        } else if(transform.position.y > yPosRange || transform.position.y < -yPosRange)
        {
            Destroy(gameObject);
        }
    }
}
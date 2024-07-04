using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private SpawnManager spawnScript;

    public float speed;

    private float yPosRange = 6;
    private float xPosRange = 9.8f;
    private float zPosRange = 14.8f;

    void Start()
    {
        spawnScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public GameObject deathCam;
    public GameObject player;
    private Animator zombieAnim;
    public bool isAlive;
    private float speed = 3;

    void Start()
    {
        zombieAnim = GetComponent<Animator>();
        isAlive = true;
    }

    void Update()
    {
        if (isAlive && GameManager.Instance.isGameActive)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject && isAlive && !collision.gameObject.CompareTag("Player"))
        {
            transform.Rotate(0, Random.Range(90, 270), 0);
        }
    }

    public void OnHit()
    {
        zombieAnim.SetTrigger("Dead");
        isAlive = false;

        StartCoroutine(DestroyZombie());
    }

    IEnumerator DestroyZombie()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    public void AttackPlayer()
    {
        LookAtDeathCam();
        zombieAnim.SetTrigger("Attack");
    }

    void LookAtDeathCam()
    {
        Vector3 direction;

        if (deathCam.transform.position.z < transform.position.z)
        {
            direction = deathCam.transform.position - transform.position;
        } else
        {
            direction = transform.position - deathCam.transform.position;
        }

        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = rotation;
        //transform.LookAt(player.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private Animator zombieAnim;
    private bool isAlive = true;
    private float speed = 3;

    void Start()
    {
        zombieAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAlive)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject && isAlive)
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
}

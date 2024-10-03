using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public GameObject deathCam;
    public GameObject player;
    private Rigidbody zombieRb;

    public ParticleSystem bloodParticle;

    private MainManager mainManager;
    private Animator zombieAnim;
    private AudioSource zombieAudio;

    public bool isAlive;
    private float speed = 3;

    void Start()
    {
        zombieAnim = GetComponent<Animator>();
        zombieAudio = GetComponent<AudioSource>();
        zombieRb = GetComponent<Rigidbody>();
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        isAlive = true;
    }

    void Update()
    {
        if (isAlive && mainManager.isGameActive)
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

    public void OnHit(Vector3 hitPoint)
    {
        zombieAnim.SetTrigger("Dead");

        bloodParticle.transform.position = hitPoint;
        bloodParticle.Play();
        
        isAlive = false;
        zombieRb.isKinematic = true;

        StartCoroutine(DestroyZombie());
    }

    IEnumerator DestroyZombie()
    {
        yield return new WaitForSeconds(2.5f);

        Destroy(gameObject);
    }

    public void AttackPlayer()
    {
        LookAtDeathCam();
        zombieAudio.Play();
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

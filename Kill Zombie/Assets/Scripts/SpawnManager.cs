using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;

    private void Start()
    {
        StartCoroutine(SpawnZombie());
    }

    IEnumerator SpawnZombie()
    {
        while(GameManager.Instance.isGameActive)
        {
            yield return new WaitForSeconds(GameManager.Instance.spawnZombieRate);

            zombiePrefab.transform.eulerAngles = new Vector3(0, Random.Range(0, 364), 0);
            Instantiate(zombiePrefab, gameObject.transform.position, zombiePrefab.transform.rotation);

            GameManager.Instance.zombieCount += 1;
        }
    }
}

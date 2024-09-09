using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    private MainManager mainManager;

    private void Start()
    {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        //StartCoroutine(SpawnZombie());
    }

    IEnumerator SpawnZombie()
    {
        while(mainManager.isGameActive && mainManager.zombieCount < 21)
        {
            yield return new WaitForSeconds(mainManager.spawnZombieRate);

            zombiePrefab.transform.eulerAngles = new Vector3(0, Random.Range(0, 364), 0);
            Instantiate(zombiePrefab, gameObject.transform.position, zombiePrefab.transform.rotation);

            mainManager.zombieCount += 1;
        }
    }
}

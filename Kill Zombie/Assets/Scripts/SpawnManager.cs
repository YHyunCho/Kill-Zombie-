using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;

    public int currentZombieCount = 1;
    private int maxZombie = 10;

    private float spawnRangeX = 3.5f;
    private float spawnRangeZ = 11;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentZombieCount < maxZombie)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, Random.Range(spawnRangeZ, spawnRangeZ + 3.17f));
        zombiePrefab.transform.eulerAngles = new Vector3(0, Random.Range(0, 364), 0);

        Instantiate(zombiePrefab, spawnPos, zombiePrefab.transform.rotation);
        currentZombieCount += 1;
    }
}

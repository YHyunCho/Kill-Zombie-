using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSight : MonoBehaviour
{
    public GameObject Zombie;
    private Vector3 offset = new Vector3(0, 1.7f, 0);

    // Update is called once per frame
    void Update()
    {
        transform.position = Zombie.transform.position + offset;
    }
}

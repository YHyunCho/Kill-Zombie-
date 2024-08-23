using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Camera aimCamera;
    public GameObject player;

    private Vector3 offset = new Vector3(0.181f, 0.696f, 1);
    private Vector3 screenCenter;
    private Vector3 startPos;

    private Ray ray;

    [SerializeField] private float speed = 50;

    private void Start()
    {
        screenCenter = new Vector3(aimCamera.pixelWidth / 2, aimCamera.pixelHeight / 2);
        startPos = player.transform.position + offset;
        ray = new Ray(startPos, screenCenter);
    }

    void Update()
    {
        transform.position += ray.direction * speed * Time.deltaTime;
    }
}

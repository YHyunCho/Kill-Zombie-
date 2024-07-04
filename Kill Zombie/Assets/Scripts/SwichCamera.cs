using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichCamera : MonoBehaviour
{
    public Camera thirdView;
    public Camera firstView;
    public Camera deathView;

    public Canvas crosshair;

    private PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        thirdView.enabled = true;
        firstView.enabled = false;
        deathView.enabled = false;

        crosshair.enabled = false;
    }

    void Update()
    {
        if (playerControllerScript.gameOver == true)
        {
            thirdView.enabled = false;
            firstView.enabled = false;
            deathView.enabled = true;

            crosshair.enabled = false;
        } else if (Input.GetMouseButton(1))
        {
            thirdView.enabled = false;
            firstView.enabled = true;
            deathView.enabled = false;

            crosshair.enabled = true;
        } else
        {
            thirdView.enabled = true;
            firstView.enabled = false;
            deathView.enabled = false;

            crosshair.enabled = false;
        }
    }
}

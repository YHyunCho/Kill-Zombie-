using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichCamera : MonoBehaviour
{
    public Camera thirdView;
    public Camera firstView;

    void Start()
    {
        thirdView.enabled = true;
        firstView.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            thirdView.enabled = false;
            firstView.enabled = true;
        }
        else
        {
            thirdView.enabled = true;
            firstView.enabled = false;
        }
    }
}

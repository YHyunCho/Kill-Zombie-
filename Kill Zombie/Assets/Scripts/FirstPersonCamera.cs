using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FirstPersonCamera : CameraMovement
{
    private void LateUpdate()
    {
        CameraRotation();
    }
}

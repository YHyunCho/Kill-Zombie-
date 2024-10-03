using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdPersonCamera : CameraMovement
{
    public LayerMask cameraCollision;

    private void LateUpdate()
    {
        CameraRotation();
        CheckCollision();
    }

    private void CheckCollision()
    {
        Vector3 rayDir = transform.position - player.transform.position;

        if (Physics.Raycast(player.transform.position, rayDir, out RaycastHit hitBack, range, cameraCollision))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, hitBack.point.z - rayDir.normalized.z);
        } 
    }

    public void PlayerWinReaction()
    {
        Vector3 offset = new Vector3(0, 2, 1.3f);
        Vector3 rotation = new Vector3(40, 180, 0);
        Vector3 cameraPos = player.transform.position + offset;

        transform.DOMove(cameraPos, 1f);
        transform.DORotate(rotation, 1f);
    }

}

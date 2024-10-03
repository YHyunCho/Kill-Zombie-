using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CameraHandler : MonoBehaviour
{
    public GameObject player;

    public Camera firstViewCam;
    public Camera thirdViewCam;
    public Camera deathViewCam;

    public Text levelText;
    public Image crosshair;

    private Vector3 deathCamOffset = new Vector3(-0.4f, 1.09f, -1f);

    void Start()
    {
        ActivateThirdPersonCamera();

        deathViewCam.enabled = false;
        deathCamOffset = new Vector3(player.transform.position.x, deathCamOffset.y, deathCamOffset.z);
    }

    public void ActivateFirstPersonCamera()
    {
        thirdViewCam.enabled = false;
        firstViewCam.enabled = true;
        levelText.gameObject.SetActive(false);

        crosshair.enabled = true;
    }

    public void ActivateThirdPersonCamera()
    {
        thirdViewCam.enabled = true;
        firstViewCam.enabled = false;
        levelText.gameObject.SetActive(true);

        crosshair.enabled = false;
    }

    public void ActivateDeathCamera(Transform zombie)
    {
        thirdViewCam.enabled = false;
        firstViewCam.enabled = false;
        deathViewCam.enabled = true;
        levelText.gameObject.SetActive(true);

        crosshair.enabled = false;

        deathViewCam.transform.position = zombie.position + deathCamOffset;
        LookAtZombie(zombie);
    }

    public void CameraPlayerHitReaction()
    {
        Vector3 posOffset = new Vector3(-0.5f, -0.7f, 0);
        Vector3 rotation = new Vector3(0, 0, 90);
        Quaternion currentRo = deathViewCam.transform.rotation;

        Vector3 vectorCurrentRo = currentRo.eulerAngles;
        Vector3 deathRo = vectorCurrentRo + rotation;

        Vector3 deathPos = deathViewCam.transform.position + posOffset;

        deathViewCam.transform.DORotate(deathRo, 1f);
        deathViewCam.transform.DOMove(deathPos, 1f);
    }

    public void LookAtZombie(Transform zombie)
    {
        Vector3 direction = zombie.position - deathViewCam.transform.position;
        direction.y = 0;

        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = rotation;
    }
}

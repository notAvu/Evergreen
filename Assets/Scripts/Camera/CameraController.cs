using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject mainCamera;

    private void Awake()
    {
        mainCamera = Camera.allCameras[0].gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !mainCamera.transform.parent.Equals(gameObject.transform))
        {
            mainCamera.transform.parent = gameObject.transform;
            mainCamera.transform.localPosition = new Vector3(0, 0, mainCamera.transform.position.z);
        }
    }
}
//Scripts a exportar CameraController, PlayerController(PLayerTpAnimation set ableToShoot to false during animation) , GunPoint, TeleportProjectile
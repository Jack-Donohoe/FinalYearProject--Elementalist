using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    public CinemachineVirtualCamera gameCamera;
    public GameObject lookPoint;
    public float cameraRotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        gameCamera.transform.RotateAround(lookPoint.transform.position, Vector3.up, cameraRotationSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraSetup : MonoBehaviourPun
{
    CinemachineVirtualCamera cam;

    void Start()
    {
        if (photonView.IsMine)
        {
            cam = FindObjectOfType<CinemachineVirtualCamera>();
            cam.LookAt = transform;
            cam.Follow = transform;
        }
    }
}

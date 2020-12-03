﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnBilliardBall : MonoBehaviour
{
    public GameObject balls;
    public Transform spawnPosition;

    private GameObject spawnedBalls;

    public float CushionFriction = 0.2F;

    void Start()
    {
        //SpawnBall();
    }

    public void SpawnBall()
    {
        if (spawnedBalls != null)
        {
            PhotonNetwork.Destroy(spawnedBalls);
        }

        spawnedBalls = PhotonNetwork.Instantiate("8Balls", spawnPosition.position, Quaternion.Euler(0, 0, 0));
        spawnedBalls.transform.position = spawnPosition.position;
    }

    [PunRPC]
    void cushionRPC(int otherPVID, Vector3 normal)
    {
        Collider other = PhotonNetwork.GetPhotonView(otherPVID).GetComponent<Collider>();
        other.attachedRigidbody.velocity = Vector3.Reflect(other.attachedRigidbody.velocity, normal) * CushionFriction;
        other.attachedRigidbody.angularVelocity = Vector3.Reflect(other.attachedRigidbody.angularVelocity, normal) * CushionFriction;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public float maxSpeed = 50f;

    Vector3 currentPos;
    Rigidbody rig;

    float slipperyTime = 2f;
    float decreaseTime = 0.01f;

    float collisionTime = 0;
    float decreaseTempTime = 0;

    HockeyManager hockeyManager;
        

    // Start is called before the first frame update
    void Start()
    {
        hockeyManager = GetComponentInParent<HockeyManager>();
        currentPos = GetComponent<Transform>().position;
        rig = GetComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeRotation;
        GetComponent<Collider>().material.dynamicFriction = 0;
        //Physics.IgnoreCollision(GetComponent<Collider>(), GetComponentInParent<Collider>());

    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
        if(Mathf.Abs(rig.velocity.x) > maxSpeed)
        {
            if(rig.velocity.x > 0)
                rig.velocity = new Vector3(maxSpeed, 0, rig.velocity.z);
            else
                rig.velocity = new Vector3(-maxSpeed, 0, rig.velocity.z);

        }
        if(Mathf.Abs(rig.velocity.z) > maxSpeed)
        {
            if(rig.velocity.z>0)
                rig.velocity = new Vector3(rig.velocity.x, 0, maxSpeed);
            else
                rig.velocity = new Vector3(rig.velocity.x, 0, -maxSpeed);
        }

        Debug.Log(rig.velocity);


        if(transform.position.y != currentPos.y)
        {
            transform.position = new Vector3(transform.position.x, currentPos.y, transform.position.z);
        }
        if (Time.time - collisionTime > slipperyTime)
        {
            if (Time.time - decreaseTempTime > decreaseTime)
            {
                rig.velocity = new Vector3(rig.velocity.x * 0.99f, 0, rig.velocity.z * 0.99f);
                decreaseTime = Time.time;
            }
        }


    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Wall")
        {
            if(collider.gameObject.name == "Wall1")
            {
                rig.velocity = new Vector3(rig.velocity.x, 0, -rig.velocity.z);
            }
            if (collider.gameObject.name == "Wall2")
            {
                rig.velocity = new Vector3(-rig.velocity.x, 0, rig.velocity.z);
            }
            if (collider.gameObject.name == "Score")
            {
                hockeyManager.Score(true);
                Destroy(this.gameObject);
            }
            if (collider.gameObject.name == "Score2")
            {
                hockeyManager.Score(false);
                Destroy(this.gameObject);
            }
            if (collider.gameObject.tag != "HockeyGround")
            {
                collisionTime = Time.time;
            }
        }
    }
}

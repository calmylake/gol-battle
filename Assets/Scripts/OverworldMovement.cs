using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{

    float movementX;
    float movementZ;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move(movementX, movementZ);
    }

    void Update()
    {

        movementZ = 0;

        if (Input.GetKey("up"))
        {
            movementZ += 1;
        }
        if (Input.GetKey("down"))
        {
            movementZ += -1;
        }

        movementX = 0;

        if (Input.GetKey("right"))
        {
            movementX += 1;
        }
        if (Input.GetKey("left"))
        {
            movementX += -1;
        }
    }

    private void Move(float x, float z)
    {
        int moveUnity = 100;

        x *= moveUnity * Time.deltaTime;
        z *= moveUnity * Time.deltaTime;

        rb.velocity = new Vector3(x, 0, z);
    }
}
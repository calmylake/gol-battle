using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    
    //movement
    bool isMoving;
    Vector3 finalPos;
    float MovementDelay;
    float MovementPrecision;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
            if(finalPos.x - transform.position.x < MovementPrecision && finalPos.z - transform.position.z < MovementPrecision && finalPos.y - transform.position.y < MovementPrecision)
            {
                isMoving = false;
            }
        }
    }

    public void AskMovement(Vector3 pos, float movementDelay)
    {
        finalPos = pos;
        MovementDelay = movementDelay;
        isMoving = true;
    }

    public void Move()
    {
        transform.position += (finalPos - transform.position)/MovementDelay;
    }
}

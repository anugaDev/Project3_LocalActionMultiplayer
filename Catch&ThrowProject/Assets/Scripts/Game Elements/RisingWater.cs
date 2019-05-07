using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private Rigidbody rb;

    public float maxHeight;
    public float minHeight;
    public float actionTimer;
    public float cooldownTime;
    public float waterTime;

    public float riseSpeed;
    private float moveDirection;

    private bool waterDown, rising, waterUp, decreasing;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        waterDown = true;
        actionTimer = 0;
        moveDirection = 0;
    }

    void FixedUpdate()
    {
        if (rising || decreasing)
        {
            rb.velocity = new Vector3(0, moveDirection, 0) * riseSpeed * Time.deltaTime;
        }

        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void Update()
    {
        if (waterDown && actionTimer < cooldownTime)
        {
            actionTimer+=0.5f;
        }

        else if (waterDown && actionTimer >= cooldownTime)
        {
            waterDown = false;
            rising = true;
            actionTimer = 0f;
            moveDirection = 1f;
        }

        if (rising)
        {
            if (gameObject.transform.position.y >= maxHeight)
            {
                rising = false;
                waterUp = true;
            }
        }

        if (waterUp)
        {
            if (actionTimer < waterTime)
            {
                actionTimer += 0.5f;
            }

            if (actionTimer >= waterTime)
            {
                actionTimer = 0f;
                waterUp = false;
                decreasing = true;
                moveDirection = -1f;
            }
        }

        if (decreasing)
        {
            if (gameObject.transform.position.y <= minHeight)
            {
                decreasing = false;
                waterDown = true;              
            }
        }
    }
}

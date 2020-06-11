using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyCustomer : Customer
{
    private float movementSpeed = 5f;
    public HappyCustomer()
    {
        this.MovementSpeed = movementSpeed;
        spotLightColor = Color.green;
    }
}

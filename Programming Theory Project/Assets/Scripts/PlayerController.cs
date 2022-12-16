using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Car
{
    protected override void FixedUpdate()
    {
        //Input Values
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Call base script
        base.FixedUpdate();
    }
}

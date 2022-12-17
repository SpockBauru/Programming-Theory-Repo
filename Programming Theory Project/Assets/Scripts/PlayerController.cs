using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Car
{
    protected override void FixedUpdate()
    {
        //Input Values
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        MoveCar(verticalInput, horizontalInput);

        if (Input.GetKeyDown(KeyCode.Space)) BreakCar();
        if (Input.GetKeyUp(KeyCode.Space)) ReleaseBreak();

        // Reset car if press R or if fall from map
        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < -20)
            ResetCar();

        // Call base script
        base.FixedUpdate();
    }
}

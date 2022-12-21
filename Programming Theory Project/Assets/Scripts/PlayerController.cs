using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Car
{
    protected override void FixedUpdate()
    {
        // Check if race is finished
        if (GameManager.Instance.HasFinished)
        {
            BrakeCar(brakeTorque);
            return;
        }

        // Move Car
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        MoveCar(verticalInput, horizontalInput);

        // Break Car
        if (Input.GetKeyDown(KeyCode.Space)) BrakeCar(brakeTorque);
        if (Input.GetKeyUp(KeyCode.Space)) ReleaseBreak();

        // Call base script
        base.FixedUpdate();
    }
}

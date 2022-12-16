using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CarControl carControl;

    private void OnEnable()
    {
        carControl = GetComponent<CarControl>();
    }

    void FixedUpdate()
    {
        //Input Values
        carControl.horizontalInput = Input.GetAxis("Horizontal");
        carControl.verticalInput = Input.GetAxis("Vertical");
    }
}

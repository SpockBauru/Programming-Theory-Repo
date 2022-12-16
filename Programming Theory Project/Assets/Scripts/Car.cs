using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Base")]
    // Editor variables
    [SerializeField] private float motorForce = 50000;
    [SerializeField] private float rotationForce = 50000f;

    [SerializeField] private float maxSpeed = 120;
    [SerializeField] private float maxRotationSpeed = 1;

    [SerializeField] private Vector3 centerOfMass = new(0, -1, 0);

    // Variables accessed by children
    protected float horizontalInput;
    protected float verticalInput;

    // Private variables
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private RaycastHit hit;
    private bool grounded = true;

    // Start is called before the first frame update
    void Awake()
    {
        //km/h to m/s
        maxSpeed /= 3.6f;

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;

        //Getting initial values
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        //check if car is not flying
        grounded = Physics.Raycast(transform.position, -transform.up, out hit, 1.3f);
        grounded = grounded && hit.transform.CompareTag("Ground");
        Debug.DrawRay(transform.position, -transform.up * 1.3f, Color.yellow);

        //Forward and backward
        if (grounded && rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(verticalInput * motorForce * transform.forward);
        }


        // Turn Right or Left
        if (grounded && rb.angularVelocity.magnitude < maxRotationSpeed)
        {
            rb.AddTorque(horizontalInput * rotationForce * transform.up);
        }
            

        // Reset if fall
        if (transform.position.y < -20)
        {
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            rb.velocity = Vector3.zero;
        }

        // Reset when press R
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            rb.velocity = Vector3.zero;
        }
    }
}

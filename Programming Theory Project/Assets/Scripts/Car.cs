using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Base")]
    // Editor variables
    [SerializeField] private float motorTorque = 1000;
    [SerializeField] private float brakeTorque = 3000;
    [SerializeField] private float steerAngle = 30f;

    [SerializeField] private float maxSpeed = 120;

    [SerializeField] private Vector3 centerOfMass = new(0, -1, 0);

    [SerializeField] private Transform frontLeft;
    [SerializeField] private Transform frontRight;
    [SerializeField] private Transform rearLeft;
    [SerializeField] private Transform rearRight;

    [SerializeField] private WheelCollider frontLeftCollider;
    [SerializeField] private WheelCollider frontRightCollider;
    [SerializeField] private WheelCollider rearLeftCollider;
    [SerializeField] private WheelCollider rearRightCollider;

    // Variables modified by children
    protected float horizontalInput;
    protected float verticalInput;

    // Private variables
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 pos;
    private Quaternion rot;

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

    // FixedUpdate is called at fixed intervals of 20ms
    protected virtual void FixedUpdate()
    {
        MoveWheels();
    }
    protected void MoveCar(float vertical, float horizontal)
    {
        if (rb.velocity.magnitude <= maxSpeed)
        {
            frontLeftCollider.motorTorque = vertical * motorTorque;
            frontRightCollider.motorTorque = vertical * motorTorque;
        }
        else
        {
            frontLeftCollider.motorTorque = 0;
            frontRightCollider.motorTorque = 0;
        }

        frontLeftCollider.steerAngle = horizontal * steerAngle;
        frontRightCollider.steerAngle = horizontal * steerAngle;
    }


    protected void BreakCar()
    {
        frontLeftCollider.brakeTorque = brakeTorque;
        frontRightCollider.brakeTorque = brakeTorque;
        rearLeftCollider.brakeTorque = brakeTorque;
        rearRightCollider.brakeTorque = brakeTorque;
    }

    protected void ReleaseBreak()
    {
        frontLeftCollider.brakeTorque = 0;
        frontRightCollider.brakeTorque = 0;
        rearLeftCollider.brakeTorque = 0;
        rearRightCollider.brakeTorque = 0;
    }

    void MoveWheels()
    {
        UpdateWheel(frontLeft, frontLeftCollider);
        UpdateWheel(frontRight, frontRightCollider);
        UpdateWheel(rearLeft, rearLeftCollider);
        UpdateWheel(rearRight, rearRightCollider);
    }

    void UpdateWheel(Transform transform, WheelCollider collider)
    {
        collider.GetWorldPose(out pos, out rot);
        transform.SetPositionAndRotation(pos, rot);
    }

    protected virtual void ResetCar()
    {
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        rb.velocity = Vector3.zero;
    }
}

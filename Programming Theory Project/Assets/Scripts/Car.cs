using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Base")]
    // Editor variables
    [SerializeField] private float motorTorque = 1000;
    [SerializeField] protected float brakeTorque = 3000;
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
    // INHERITANCE
    protected virtual void FixedUpdate()
    {
        MoveWheels();

        // Reset car if fall from map
        if (transform.position.y < -20)
        {
            StartCoroutine(ResetCar());
            return;
        }
    }
    protected void MoveCar(float vertical, float horizontal)
    {
        // If game is not started, stay still
        if (!GameManager.Instance.HasStarted)
        {
            return;
        }

        // Add force for front wheels
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

        // Steer fring wheels
        frontLeftCollider.steerAngle = horizontal * steerAngle;
        frontRightCollider.steerAngle = horizontal * steerAngle;
    }

    protected void BrakeCar(float torque)
    {
        frontLeftCollider.brakeTorque = torque;
        frontRightCollider.brakeTorque = torque;
        rearLeftCollider.brakeTorque = torque;
        rearRightCollider.brakeTorque = torque;
    }

    protected void ReleaseBreak()
    {
        frontLeftCollider.brakeTorque = 0;
        frontRightCollider.brakeTorque = 0;
        rearLeftCollider.brakeTorque = 0;
        rearRightCollider.brakeTorque = 0;
    }

    // ABSTRACTION
    private void MoveWheels()
    {
        UpdateWheel(frontLeft, frontLeftCollider);
        UpdateWheel(frontRight, frontRightCollider);
        UpdateWheel(rearLeft, rearLeftCollider);
        UpdateWheel(rearRight, rearRightCollider);
    }

    private void UpdateWheel(Transform transform, WheelCollider collider)
    {
        collider.GetWorldPose(out pos, out rot);
        transform.SetPositionAndRotation(pos, rot);
    }

    public virtual IEnumerator ResetCar()
    {
        // Reset physics
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        rb.velocity = Vector3.zero;
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
        BrakeCar(Mathf.Infinity);

        // Wait one frame to apply physics, then return to normal
        yield return null;
        ReleaseBreak();
    }
}

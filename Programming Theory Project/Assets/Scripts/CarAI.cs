using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : Car
{
    [Header("Car AI")]
    //position of the car in the lane
    public float laneOffsetX = 0f;
    public float laneOffsetZ = 0f;

    //Limit angle between car and checkpoint
    public float maxAngle = 90f;

    // Checkpoints
    private Transform currentCheckpoint;
    private int checkpointLenght;
    private int checkpointIndex = 1;

    private Vector3 checkPointPosition;
    [SerializeField] private bool checkpointFinished = false;

    // Car variables
    private Vector3 currentPosition;
    private float turnForce = 0;

    // Start is called before the first frame update
    private void Start()
    {
        checkpointLenght = CheckpointList.CheckPointList.Length;
        SetCheckpoint(checkpointIndex);
    }

    protected override void FixedUpdate()
    {
        currentPosition = transform.position;

        if (checkpointFinished)
        {
            BrakeCar(brakeTorque);
        }
        else
        {
            SetInputs(currentPosition, checkPointPosition);
            MoveCar(verticalInput, horizontalInput);
        }

        // Call base script
        base.FixedUpdate();
    }

    private void SetInputs(Vector3 carPosition, Vector3 desiredPosition)
    {
        Vector3 direction = desiredPosition - carPosition;
        float angle = Vector3.SignedAngle(transform.forward, direction, transform.up);
        turnForce = Mathf.Clamp(angle / maxAngle, -1, 1);

        horizontalInput = turnForce;
        verticalInput = 1;
    }

    public void ResetAI()
    {
        checkpointFinished = false;
        checkpointIndex = 1;
        SetCheckpoint(checkpointIndex);
        StartCoroutine(ResetCar());
    }

    private void SetCheckpoint(int i)
    {
        currentCheckpoint = CheckpointList.CheckPointList[i];

        // Position with offset on X axis scaled to wold coordinates
        checkPointPosition = currentCheckpoint.TransformPoint(laneOffsetX / currentCheckpoint.localScale.x, 0, laneOffsetZ / currentCheckpoint.localScale.z);
        Debug.DrawLine(transform.position, checkPointPosition, Color.yellow, 10f);
    }

    // Get next checkpoint
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetInstanceID() == currentCheckpoint.GetInstanceID())
        {
            checkpointIndex++;

            if (checkpointIndex < checkpointLenght)
                SetCheckpoint(checkpointIndex);
            else
            {
                checkpointFinished = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    // Public things
    //position of the car in the lane
    public float laneOffsetX = 0f;
    public float laneOffsetZ = 0f;

    //Limit angle between car and checkpoint
    public float angleWide = 90f;

    // Checkpoints
    private Transform currentCheckpoint;
    private int checkpointLenght;
    private int checkpointIndex = 1;

    private Vector3 checkPointPosition;
    private Vector3 checkpointDirection;
    private float checkpointAngle;
    private bool checkpointFinished = false;

    // Car variables
    private CarControl carControl;
    private Vector3 currentPosition;
    private float turnForce = 0;

    // OnEnable is called before Start
    private void OnEnable()
    {
        carControl = GetComponent<CarControl>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        checkpointLenght = CheckpointList.CheckPointList.Length;
        SetCheckpoint(checkpointIndex);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        currentPosition = transform.position;

        // Reset when press R
        if (Input.GetKeyDown(KeyCode.R) || currentPosition.y < -15f)
        {
            checkpointFinished = false;
            checkpointIndex = 1;
            SetCheckpoint(checkpointIndex);
        }

        // =================================== return is cool! ==============================
        if (checkpointFinished)
        {
            carControl.verticalInput = 0;
            carControl.horizontalInput = 0;
            return;
        }

        checkpointDirection = checkPointPosition - currentPosition;
        checkpointAngle = Vector3.SignedAngle(transform.forward, checkpointDirection, transform.up);

        turnForce = Mathf.Clamp(checkpointAngle / angleWide, -1, 1);

        carControl.horizontalInput = turnForce;
        carControl.verticalInput = 1;
    }

    private void SetCheckpoint(int i)
    {
        currentCheckpoint = CheckpointList.CheckPointList[i];

        // Position with offset on X axis scaled to wold coordinates
        checkPointPosition = currentCheckpoint.TransformPoint(laneOffsetX / currentCheckpoint.localScale.x, 0, laneOffsetZ / currentCheckpoint.localScale.z);
        Debug.DrawLine(transform.position, checkPointPosition, Color.yellow, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetInstanceID() == currentCheckpoint.GetInstanceID())
        {
            checkpointIndex++;

            if (checkpointIndex < checkpointLenght)
                SetCheckpoint(checkpointIndex);
            else
            {
                Debug.Log(gameObject.name + " Finished");
                checkpointFinished = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Smooth Camera Follow")]
    [SerializeField] Transform car;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float camSpeed = 10f;
    [SerializeField] private float finishRotateSpeed = 10f;

    private Vector3 position = Vector3.zero;
    private Vector3 carPosition = Vector3.zero;
    private Vector3 offset = Vector3.zero;
    private Vector3 desiredPosition = Vector3.zero;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.localPosition - car.localPosition;
    }

    // FixedUpdate is called at fixed intervals
    void FixedUpdate()
    {
        position = transform.position;
        carPosition = car.position;
        desiredPosition = carPosition + (car.up * offset.y) + (car.forward * offset.z);

        // Check if race is finished
        if (GameManager.Instance.HasFinished)
        {
            float distance = (position - carPosition).magnitude;
            float desiredDistance = (desiredPosition - carPosition).magnitude + 1;
            if (distance <= desiredDistance)
            {
                transform.RotateAround(carPosition, Vector3.up, Time.fixedDeltaTime * finishRotateSpeed);
                return;
            }
        }

        //if distance is within the range, do the lerp, otherwise immediately set position
        distance = (carPosition - position).magnitude;
        if (distance < maxDistance)
            position = Vector3.Lerp(position, desiredPosition, camSpeed * Time.fixedDeltaTime);
        else
            position = desiredPosition;

        transform.position = position;
        transform.LookAt(carPosition);
    }
}

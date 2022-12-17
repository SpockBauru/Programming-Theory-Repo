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

    Vector3 offset= Vector3.zero;
    Vector3 desiredPosition= Vector3.zero;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - car.position;
    }

    // FixedUpdate is called at fixed intervals of 20ms
    void FixedUpdate()
    {
        desiredPosition = car.position + (car.forward * offset.z) + (car.up * offset.y);

        //if distance is within the range, do the lerp, otherwise immediately set position
        distance = (car.position - transform.position).magnitude;
        if (distance < maxDistance)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, camSpeed * Time.fixedDeltaTime);
        else
            transform.position = desiredPosition;

        transform.LookAt(car.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointList : MonoBehaviour
{
    public static Transform[] CheckPointList { get; private set; }

    // OnEnable is called before Start
    private void OnEnable()
    {
        CheckPointList = GetComponentsInChildren<Transform>();
    }
}

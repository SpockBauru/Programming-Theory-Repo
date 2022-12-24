using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class TunnelShadowColor : MonoBehaviour
{
    [SerializeField] private Color shadowColor = Color.black;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = RenderSettings.subtractiveShadowColor;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.name == "Player")
        {
            RenderSettings.subtractiveShadowColor = shadowColor;
            Debug.Log("TriggerEnter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.name == "Player")
        {
            RenderSettings.subtractiveShadowColor = originalColor;
            Debug.Log("TriggerExit");
        }
    }
}

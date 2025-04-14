using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.Gyroscope;


public class Paddle : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Vector3 yAxis = new Vector3(0, 1, 0);
    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogError("Gyro not supported");
            Input.gyro.enabled = false;
        }
    }
    void Update()
    {
        if (Input.gyro.enabled)
        {
            Quaternion gyroRotation = Input.gyro.attitude;
            Vector3 unityEuler = gyroRotation.eulerAngles;
        
            float deltaY = unityEuler.y;
            float normalizedY = Mathf.Clamp(deltaY / 90f, -1f, 1f);
        
            transform.Translate(0f, normalizedY * speed * Time.deltaTime, 0f);
        }
    }
}

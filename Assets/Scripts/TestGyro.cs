using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGyro : MonoBehaviour
{
    void Start()
    {
        Input.compass.enabled = true;
        Input.gyro.enabled = true;
    }

    void Update()
    {
        // Esto es la dirección "plana" hacia el norte
        float heading = Input.compass.trueHeading;

        // Esto es la orientación completa del dispositivo (3D)
        Quaternion deviceRotation = Input.gyro.attitude;

        // Ajuste para invertir ejes en Android
        Quaternion correctedRotation = new Quaternion(
            -deviceRotation.x,
            -deviceRotation.y,
            deviceRotation.z,
            deviceRotation.w);

        // Aplica la rotación a un objeto en la escena
        transform.rotation = correctedRotation;

        // Solo para debug
        Debug.Log($"Brújula: {heading}°, Rotación: {correctedRotation.eulerAngles}");
        Debug.Log(Input.acceleration);
    }
}

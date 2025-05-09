using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cmara : MonoBehaviour
{
    [SerializeField] Camera cam;
    void Start()
    {
       /* SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        Vector2 worldSize = sr.bounds.size;
        
        Vector3 worldPos1 = sr.bounds.min;
        Vector3 worldPos2 = sr.bounds.max;

        Vector3 screenPos1 = cam.WorldToScreenPoint(worldPos1);
        Vector3 screenPos2 = cam.WorldToScreenPoint(worldPos2);

        Vector2 screenSize = screenPos2 - screenPos1;

        Debug.Log("Tamaño en píxeles en pantalla: " + screenSize);
        */
       
       RectTransform rt = GetComponent<RectTransform>();
       if (rt == null)
       {
           Debug.LogError("No RectTransform encontrado.");
           return;
       }

       // Tamaño local en píxeles (según el CanvasScaler)
       Vector2 size = rt.rect.size;

       Debug.Log("Tamaño en píxeles UI local: " + size);

    }

}

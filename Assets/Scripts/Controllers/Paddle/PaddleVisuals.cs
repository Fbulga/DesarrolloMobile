using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleVisuals : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.IsMobilePlatform)
        {
            gameObject.GetComponent<GhostSprites>().enabled = false;
            gameObject.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<GhostSprites>().enabled = true;
            gameObject.GetComponent<TrailRenderer>().enabled = false;
        }
    }

}

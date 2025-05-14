using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleEffect : MonoBehaviour
{
    private Vector3 originalScale;
    private PaddleMovement paddle;
    private float originalRayDistance;
    private void Start()
    {
        paddle = GetComponent<PaddleMovement>();
        originalScale = transform.localScale;
        originalRayDistance = paddle.paddleData.RayDistance;
    }

    //LongPaddle
    public void LongPaddle(float scaleMultiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleCoroutine(scaleMultiplier, duration));
    }
    private IEnumerator ScaleCoroutine(float multiplier, float duration)
    {
        if(paddle.LeftMovement == 0 && originalScale == transform.localScale)
        {
            Debug.Log("entra mov izq");
            transform.position = new Vector3(transform.position.x+multiplier/4.2f,transform.position.y);
            
        }
        else if(paddle.RightMovement == 0 && originalScale == transform.localScale)
        {
            Debug.Log("entra mov der");
            transform.position = new Vector3(transform.position.x-multiplier/4.2f,transform.position.y);
        }

        transform.localScale = new Vector3(originalScale.x, originalScale.y  * multiplier, originalScale.z);
        if (paddle.rayDistance < originalRayDistance * multiplier)
        {
            paddle.rayDistance *= multiplier;
        }

        yield return new WaitForSeconds(duration);
        transform.localScale = originalScale;
        paddle.rayDistance = originalRayDistance;
    }

    

}

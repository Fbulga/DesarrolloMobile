using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleEffect : MonoBehaviour
{
    private Vector3 originalScale;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    private void Start()
    {
        originalScale = transform.localScale;
    }

    //LongPaddle
    public void LongPaddle(float scaleMultiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleCoroutine(scaleMultiplier, duration));
    }
    private IEnumerator ScaleCoroutine(float multiplier, float duration)
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y  * multiplier, originalScale.z);
        yield return new WaitForSeconds(duration);
        transform.localScale = originalScale;
    }


    public void NewBall()
    {
        Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
    }

}

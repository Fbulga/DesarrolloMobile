using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public bool IsDeadly = true;
    private float timer = 0f;
    private float deadlyTime;
    
    private Color originalColor;
    [SerializeField] private Color safeColor;

    
    protected virtual void Start()
    {
        ArkanoidManager.Instance.SetDeadZone(this);
        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
    }
    public void RunDeadlyTimer(float duration)
    { 
        Debug.Log("RunDeadlyTimer");
        deadlyTime = duration;
        IsDeadly = false;
        timer = 0f;
    }
    
    private void Update()
    {
        if (!IsDeadly)
        {
            gameObject.GetComponent<SpriteRenderer>().color = safeColor;
            timer += Time.deltaTime;
            if (timer >= deadlyTime)
            {
                gameObject.GetComponent<SpriteRenderer>().color = originalColor;
                IsDeadly = true;
            }
        }
    }
}

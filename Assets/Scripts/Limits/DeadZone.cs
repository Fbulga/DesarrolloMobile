using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public bool IsDeadly = true;
    private float timer = 0f;
    private float deadlyTime;

    public void RunDeadlyTimer(float duration)
    { 
        Debug.Log("RunDeadlyTimer");
        deadlyTime = duration;
        IsDeadly = false;
        timer = 0f;
    }

    private void Start()
    {
        GameManager.Instance.SetDeadZone(this);
    }

    private void Update()
    {
        if (!IsDeadly)
        {
            timer += Time.deltaTime;
            Debug.Log("DeadZone Deactivated");
            if (timer >= deadlyTime)
            {
                Debug.Log("Dead Zone Activated");
                IsDeadly = true;
            }
        }
    }
}

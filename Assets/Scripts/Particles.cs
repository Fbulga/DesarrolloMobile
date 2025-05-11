using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    private void OnDisable()
    {
        PoolManager.Instance.ReturnPowerUp(gameObject,particlePrefab);
    }
}

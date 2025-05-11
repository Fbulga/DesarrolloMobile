using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private BrickData particlePrefab;
    private void OnDisable()
    {
        PoolManager.Instance.ReturnPowerUp(gameObject,particlePrefab.ParticlesPrefab);
    }
}

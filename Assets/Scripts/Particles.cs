using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using Unity.VisualScripting;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public PrefabsType prefabType;

    private void OnDisable()
    {
        PoolManager.Instance.ReturnPowerUp(this.gameObject,prefabType);
    }
}

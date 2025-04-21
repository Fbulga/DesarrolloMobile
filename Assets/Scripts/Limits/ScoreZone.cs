using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    [SerializeField] private bool isPlayer;

    public bool IsPlayer => isPlayer;
}

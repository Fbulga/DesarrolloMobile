using UnityEngine;

public abstract class EffectSO : ScriptableObject, IEffectable
{
    public abstract void Execute(GameObject target);
}
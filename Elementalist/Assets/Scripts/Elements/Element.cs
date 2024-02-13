using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : ScriptableObject
{
    public abstract GameObject GetProjectile();

    public abstract float GetProjectileSpeed();

    public abstract int GetDamageValue();

    public abstract string GetAttackName();
}

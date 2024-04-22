using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : ScriptableObject
{
    public abstract string GetName();
    
    public abstract GameObject GetProjectile();

    public abstract float GetProjectileSpeed();

    public abstract int GetDamageValue();
    
    public abstract int GetMagicCost();

    public abstract string GetAttackName();

    public abstract Texture2D GetIcon();

    public abstract AudioClip GetSFX();
}

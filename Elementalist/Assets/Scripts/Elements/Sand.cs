using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sand", menuName = "Elements/Sand", order = 10)]
public class Sand : Element
{
    [SerializeField] private string element_name;
    
    [SerializeField] private GameObject sandball;
    
    [SerializeField] private float speed;

    [SerializeField] private int damage_value;
    
    [SerializeField] private int magic_cost;

    [SerializeField] private string attack_name;

    [SerializeField] private Texture2D icon;
    
    [SerializeField] private AudioClip SFX;

    public override string GetName()
    {
        return element_name;
    }

    public override GameObject GetProjectile()
    {
        return sandball;
    }

    public override float GetProjectileSpeed()
    {
        return speed;
    }

    public override int GetDamageValue()
    {
        return damage_value;
    }

    public override int GetMagicCost()
    {
        return magic_cost;
    }

    public override string GetAttackName()
    {
        return attack_name;
    }

    public override Texture2D GetIcon()
    {
        return icon;
    }
    
    public override AudioClip GetSFX()
    {
        return SFX;
    }
}

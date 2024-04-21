using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Air", menuName = "Elements/Air", order = 4)]
public class Air : Element
{
    [SerializeField] private string element_name;
    
    [SerializeField] private GameObject airball;
    
    [SerializeField] private float speed;

    [SerializeField] private int damage_value;
    
    [SerializeField] private int magic_cost;
    
    [SerializeField] private string attack_name;
    
    [SerializeField] private Texture2D icon;
    
    public override string GetName()
    {
        return element_name;
    }

    public override GameObject GetProjectile()
    {
        return airball;
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
}

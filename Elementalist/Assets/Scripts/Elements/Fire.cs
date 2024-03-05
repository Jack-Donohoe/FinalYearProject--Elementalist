using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire", menuName = "Elements/Fire", order = 1)]
public class Fire : Element
{
    [SerializeField] private string element_name;
    
    [SerializeField] private GameObject fireball;
    
    [SerializeField] private float speed;

    [SerializeField] private int damage_value;

    [SerializeField] private string attack_name;

    public override string GetName()
    {
        return element_name;
    }

    public override GameObject GetProjectile()
    {
        return fireball;
    }

    public override float GetProjectileSpeed()
    {
        return speed;
    }

    public override int GetDamageValue()
    {
        return damage_value;
    }

    public override string GetAttackName()
    {
        return attack_name;
    }
}

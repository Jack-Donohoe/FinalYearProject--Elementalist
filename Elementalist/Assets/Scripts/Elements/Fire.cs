using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire", menuName = "Elementalist/Fire", order = 1)]
public class Fire : Element
{
    [SerializeField] private GameObject fireball;

    [SerializeField] private int damage_value;
    
    public override GameObject GetProjectile()
    {
        return fireball;
    }

    public override int GetDamageValue()
    {
        return damage_value;
    }
}

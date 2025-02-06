using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponControl : MonoBehaviour
{
    public WeaponType type;
    
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;

    public void Attack()
    {
        
    }
}

public enum WeaponType
{
    Range, Melee
}

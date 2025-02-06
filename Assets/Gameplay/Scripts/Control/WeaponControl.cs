using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponControl : MonoBehaviour
{
    public WeaponType type;
    
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;

    [Title("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    
    public void Attack()
    {
        
    }
}

public enum WeaponType
{
    Range, Melee
}

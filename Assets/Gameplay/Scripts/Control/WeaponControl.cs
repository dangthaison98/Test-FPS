using System;
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
        switch (type)
        {
            case WeaponType.Range:
                PoolManager.Instance.Spawn("Bullet", bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                break;
            case WeaponType.Melee:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }  
    }
}

public enum WeaponType
{
    Range, Melee
}

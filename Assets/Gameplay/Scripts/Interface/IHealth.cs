using UnityEngine;

public interface IHealth
{
    void TakeDamage(float damage, GameObject damageSource);
    void Heal(float healAmount);
    void Kill();
}

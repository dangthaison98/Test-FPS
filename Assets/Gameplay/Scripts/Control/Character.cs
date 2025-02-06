using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, IHealth
{
    [Tooltip("Maximum amount of health")] public float MaxHealth = 10f;

    public UnityAction<float, GameObject> OnDamaged;
    public UnityAction<float> OnHealed;
    public UnityAction OnDie;

    public float CurrentHealth { get; set; }
    public bool Invincible { get; set; }

    private bool m_IsDead;

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void Heal(float healAmount)
    {
        float healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        float trueHealAmount = CurrentHealth - healthBefore;
        if (trueHealAmount > 0f)
        {
            OnHealed?.Invoke(trueHealAmount);
        }
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (Invincible)
            return;

        float healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        float trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0f)
        {
            OnDamaged?.Invoke(trueDamageAmount, damageSource);
        }

        HandleDeath();
    }

    public void Kill()
    {
        CurrentHealth = 0f;

        OnDamaged?.Invoke(MaxHealth, null);

        HandleDeath();
    }

    void HandleDeath()
    {
        if (m_IsDead)
            return;

        if (CurrentHealth <= 0f)
        {
            m_IsDead = true;
            OnDie?.Invoke();
        }
    }
}

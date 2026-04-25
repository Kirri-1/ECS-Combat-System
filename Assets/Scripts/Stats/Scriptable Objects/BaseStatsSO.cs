using UnityEngine;

public abstract class BaseStatsSO : ScriptableObject
{
    [Header("Identity")]
    public string Name;

    [Header("Health")]
    public float MaxHealth;
    public float StartingHealth;
    public float HealthRegenRate;
}

using UnityEngine;

public abstract class CombatantStatsSO : BaseStatsSO
{
    [Header("Shield")]
    public float MaxShield;
    public float StartingShield;
    public float ShieldDecayRate;

    [Header("Defense")]
    public float BaseDefense;

    [Header("Attack")]
    public float BaseAttack;

    [Header("Speed")]
    public float BaseSpeed;
    public float MaxSpeed;

    [Header("Mana")]
    public float MaxMana;
    public float StartingMana;
}

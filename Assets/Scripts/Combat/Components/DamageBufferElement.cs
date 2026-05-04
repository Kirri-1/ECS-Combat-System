using Unity.Entities;

public struct DamageBufferElement : IBufferElementData
{
    public float Amount;
    public float CriticalMultiplier;
    public bool IsCritical;
    public bool IsPercentageOfTarget;
    public bool IsPercentageOfSelf;
    public bool IgnoreShield;
    public bool IgnoreDefense;
    public float MinimumDamage;
    public DamageType Type;
    public Entity source;
}

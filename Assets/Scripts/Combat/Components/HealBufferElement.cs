using Unity.Entities;

public struct HealBufferElement : IBufferElementData
{
    public float Amount;
    public float MinimumHeal;
    public float CriticalMultiplier;
    public bool CanOverHealToShields;
    public bool IsPercentage;
    public bool IsCritical;
    public bool IgnoreHealReduction;
    public bool IgnoreHealDebuffs;
    public HealType Type;
    public Entity Source;
}

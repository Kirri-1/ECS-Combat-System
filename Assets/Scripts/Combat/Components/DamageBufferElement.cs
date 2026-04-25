using Unity.Entities;

public struct DamageBufferElement : IBufferElementData
{
    public float Amount;
    public bool IgnoreShield;
    public bool IgnoreDefense;
    public float MinimumDamage;
    public DamageType Type;
}

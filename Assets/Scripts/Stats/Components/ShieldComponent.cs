using Unity.Entities;

public struct ShieldComponent : IComponentData
{
    public float Current;
    public float Max;

    public float DecayRate;
}

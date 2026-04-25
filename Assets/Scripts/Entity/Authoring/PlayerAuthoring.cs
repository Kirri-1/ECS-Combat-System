using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public class PlayerAuthoring : CombatantStatsAuthoring 
{}

class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

        AddComponent(entity, new HealthComponent
        {
            Current = authoring.StatsSO.StartingHealth,
            Max = authoring.StatsSO.MaxHealth,
            RegenRate = authoring.StatsSO.HealthRegenRate
        });
        AddComponent(entity, new ShieldComponent
        {
            Current = authoring.StatsSO.StartingShield,
            Max = authoring.StatsSO.MaxShield,
            DecayRate = authoring.StatsSO.ShieldDecayRate
        });
        AddComponent(entity, new DefenseComponent
        {
            Current = authoring.StatsSO.BaseDefense
        });
        AddComponent(entity, new SpeedComponent
        {
            Current = authoring.StatsSO.BaseSpeed,
            Max = authoring.StatsSO.MaxSpeed
        });
        AddBuffer<DamageBufferElement>(entity);

        AddComponent(entity, new PlayerTag());
        AddComponent(entity, new PooledTag());

        var colliderBlob = SphereCollider.Create(
            new SphereGeometry
            {
                Center = float3.zero,
                Radius = 0.5f
            },
            new CollisionFilter
            {
                BelongsTo = PhysicsLayers.Player,
                CollidesWith = PhysicsLayers.All
            });

        AddComponent(entity, new PhysicsCollider { Value = colliderBlob });

    }
}

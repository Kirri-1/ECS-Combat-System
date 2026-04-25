using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;

public class CombatantStatsAuthoring : MonoBehaviour
{
    public CombatantStatsSO StatsSO;
}

class CombatantStatsBaker : Baker<CombatantStatsAuthoring>
{
    public override void Bake(CombatantStatsAuthoring authoring)
    {
        if (authoring.StatsSO == null)
        {
            Debug.LogError($"{authoring.gameObject.name}: No StatsSO assigned.");
            return;
        }

        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

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
            Max = authoring.StatsSO.MaxSpeed,
        });

        AddComponentObject(entity, new EntityGameObjectLink
        {
            GameObject = authoring.gameObject
        });

        AddBuffer<DamageBufferElement>(entity);

        var colliderBlob = Unity.Physics.SphereCollider.Create(
            new SphereGeometry
            {
                Center = float3.zero,
                Radius = 0.5f
            },
            new CollisionFilter
            {
                BelongsTo = PhysicsLayers.Mob,
                CollidesWith = PhysicsLayers.All
            });

        AddComponent(entity, new PhysicsCollider { Value = colliderBlob });

        AddComponent(entity, new MobTag());
    }
}



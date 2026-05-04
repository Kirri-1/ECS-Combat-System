using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(ShieldDecay))]
partial struct DamageSystem : ISystem
{
    private ComponentLookup<ShieldComponent> _shieldLookup;
    private ComponentLookup<DefenseComponent> _defenseLookup;
    private ComponentLookup<HealthComponent> _sourceHealthLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _shieldLookup = state.GetComponentLookup<ShieldComponent>(false);
        _defenseLookup = state.GetComponentLookup<DefenseComponent>(true);

        _sourceHealthLookup = state.GetComponentLookup<HealthComponent>(true);
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _shieldLookup.Update(ref state);
        _defenseLookup.Update(ref state);

        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (damageBuffer, health, entity) in SystemAPI.Query<
            DynamicBuffer<DamageBufferElement>,
            RefRW<HealthComponent>>()
        .WithEntityAccess())
        {
            bool hasShield = _shieldLookup.HasComponent(entity);
            bool hasDefense = _defenseLookup.HasComponent(entity);
            _sourceHealthLookup.Update(ref state);

            float shieldValue = hasShield ? _shieldLookup[entity].Current : 0;
            float defenseValue = hasDefense ? _defenseLookup[entity].Current : 0;
            float initialShield = shieldValue;

            foreach(var damage in damageBuffer)
            {
                float calculatedDamage = damage.Amount;
                if (damage.IsPercentageOfTarget)
                {
                    calculatedDamage = health.ValueRW.Current * damage.Amount;
                }
                else if (damage.IsPercentageOfSelf && _sourceHealthLookup.HasComponent(damage.source))
                {
                    calculatedDamage = _sourceHealthLookup[damage.source].Current * damage.Amount;
                }

                if(damage.IsCritical)
                {
                    calculatedDamage *= damage.CriticalMultiplier;
                }

                    float remaining = calculatedDamage;

                if(!damage.IgnoreShield && shieldValue >0)
                {
                    float absorbed = math.min(remaining, shieldValue);
                    shieldValue -= absorbed;
                    remaining -= absorbed;
                }

                if(!damage.IgnoreDefense && remaining > 0)
                {
                    remaining = math.max(remaining - defenseValue, damage.MinimumDamage);
                }

                if (remaining <= 0)
                    continue;

                health.ValueRW.Current = math.max(health.ValueRW.Current - remaining, 0);
            }
            if(hasShield && shieldValue != initialShield)
            {
                var shield = _shieldLookup[entity];
                shield.Current = shieldValue;
                _shieldLookup[entity] = shield;
            }

            if (health.ValueRW.Current <= 0)
            {
                ecb.AddComponent<DeadTag>(entity);
            }
            damageBuffer.Clear();
        }
    }
}

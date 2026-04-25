using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(ShieldDecay))]
public partial struct HealthRegen : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var healthRegen = new HealthRegenJob { DeltaTime = SystemAPI.Time.DeltaTime };
        state.Dependency = healthRegen.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
[WithNone(typeof(InCombatTag))]
partial struct HealthRegenJob : IJobEntity
{
    public float DeltaTime;

    public void Execute(ref HealthComponent health)
    {
        if (health.Current >= health.Max)
            return;

        if (health.RegenRate <= 0)
            return;

        health.Current = math.min(health.Current + health.RegenRate * DeltaTime, health.Max);
    }
}



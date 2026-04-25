using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ShieldDecay : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        var decayJob = new ShieldDecayJob { DeltaTime = SystemAPI.Time.DeltaTime, Ecb = ecb };
        state.Dependency = decayJob.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
[WithPresent(typeof(ShieldDecayTag))]
public partial struct ShieldDecayJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter Ecb;
    public void Execute(ref ShieldComponent shield, Entity entity, [ChunkIndexInQuery] int sortKey)
    {
        if (shield.DecayRate <= 0)
            return;

        shield.Current = math.max(shield.Current - shield.DecayRate * DeltaTime, 0);

        if(shield.Current <= 0)
        {
            Ecb.RemoveComponent<ShieldDecayTag>(sortKey, entity);
        }
    }
}
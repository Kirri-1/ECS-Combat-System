using UnityEngine;
using Unity.Burst;
using Unity.Entities;
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(HealthRegen))]
partial struct DeathSystem : ISystem
{
    private ComponentLookup<PooledTag> _poolTagLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _poolTagLookup = state.GetComponentLookup<PooledTag>(true);
    }

    //[BurstCompile] //commented out to allow Debug.Log
    public void OnUpdate(ref SystemState state)
    {
        _poolTagLookup.Update(ref state);

        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (dead, entity) in SystemAPI.Query<DeadTag>().WithEntityAccess())
        {
            bool isPooled = _poolTagLookup.HasComponent(entity);

            if(isPooled)
            {
                Debug.Log("Entity will be pooled!");
                ecb.RemoveComponent<DeadTag>(entity);
                ecb.AddComponent<Disabled>(entity);
            }
            else
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}

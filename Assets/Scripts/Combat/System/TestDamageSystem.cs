using Unity.Burst;
using Unity.Entities;

partial struct TestDamageSystem : ISystem
{
    private float _timer;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _timer += SystemAPI.Time.DeltaTime;
        
        if(_timer >= 1.0f)
        {
            _timer = 0;

            foreach(var (buffer, entity) in SystemAPI.Query<DynamicBuffer<DamageBufferElement>>().WithEntityAccess())
            {
                buffer.Add(new DamageBufferElement
                {
                    Amount = 5f,
                    MinimumDamage = 1f,
                    IgnoreDefense = false,
                    IgnoreShield = false
                });
            }
        }
    }

}

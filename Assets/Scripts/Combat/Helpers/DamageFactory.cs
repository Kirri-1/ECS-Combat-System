using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public static class DamageFactory
{
    public static DamageBufferElement Physical(float amount) => new DamageBufferElement
    {
        Amount = amount,
        IgnoreShield = false,
        IgnoreDefense = false,
        MinimumDamage = 1,
        Type = DamageType.Physical,
    };

    public static DamageBufferElement True(float amount) => new DamageBufferElement
    {
        Amount = amount,
        IgnoreShield = true,
        IgnoreDefense = true,
        MinimumDamage = 0,
        Type = DamageType.True,
    };


    public static void AOE(float amount, bool ignoreShield, bool ignoreDefense, float minimumDamage, DamageType type, float3 center, float radius, ref CollisionWorld collisionWorld, BufferLookup<DamageBufferElement> bufferLookup, CollisionFilter filter)
    {
        var hits = new NativeList<DistanceHit>(Allocator.Temp);

        if(collisionWorld.OverlapSphere(center, radius, ref hits, filter))
        {
            foreach(var hit in hits)
            {
                if(bufferLookup.HasBuffer(hit.Entity))
                {
                    bufferLookup[hit.Entity].Add(new DamageBufferElement
                    {
                        Amount = amount,
                        IgnoreShield = ignoreShield,
                        IgnoreDefense = ignoreDefense,
                        MinimumDamage = minimumDamage,
                        Type = type,
                    });
                }
            }
        }
        hits.Dispose();
    }
}

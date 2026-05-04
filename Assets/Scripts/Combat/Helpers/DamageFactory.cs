using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public static class DamageFactory
{
    public static DamageBufferElement Physical(float amount, Entity _source) => new DamageBufferElement
    {
        Amount = amount,
        IgnoreShield = false,
        IgnoreDefense = false,
        MinimumDamage = 1,
        Type = DamageType.Physical,
        source = _source,
    };

    public static DamageBufferElement True(float amount, Entity _source) => new DamageBufferElement
    {
        Amount = amount,
        IgnoreShield = true,
        IgnoreDefense = true,
        MinimumDamage = 0,
        Type = DamageType.True,
        source = _source,
    };


    public static void AOE(float amount, bool ignoreShield, bool ignoreDefense, float minimumDamage, DamageType type, float3 center, float radius, ref CollisionWorld collisionWorld, BufferLookup<DamageBufferElement> bufferLookup, CollisionFilter filter, Entity _source)
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
                        source = _source
                    });
                }
            }
        }
        hits.Dispose();
    }

    public static DamageBufferElement Hazard(float amount, bool ignoreShield, bool ignoreDefense, float minimumDamage, DamageType type) => new DamageBufferElement
    {
        Amount = amount,
        IgnoreShield = ignoreShield,
        IgnoreDefense = ignoreDefense,
        MinimumDamage = minimumDamage,
        Type = type,
    };
}

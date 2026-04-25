using UnityEngine;

public static class PhysicsLayers
{
    public const uint Player = 1u << 0;
    public const uint Mob = 1 << 1;
    public const uint Destructible = 1 << 2;
    public const uint Projectile = 1 << 3;
    public const uint All = uint.MaxValue;
}

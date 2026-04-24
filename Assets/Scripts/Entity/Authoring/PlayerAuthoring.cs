using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : CombatantStatsAuthoring 
{}

class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        AddComponent(entity, new PlayerTag());
    }
}

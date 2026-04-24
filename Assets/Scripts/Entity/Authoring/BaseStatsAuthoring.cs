using UnityEngine;
using Unity.Entities;

public class BaseStatsAuthoring : MonoBehaviour
{
    public BaseStatsSO StatsSO;
}

class BaseStatsBaker : Baker<BaseStatsAuthoring>
{
    public override void Bake(BaseStatsAuthoring authoring)
    {
        if (authoring.StatsSO == null)
        {
            Debug.LogError($"{authoring.gameObject.name}: No StatsSO assigned");
            return;
        }

        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        AddComponent(entity, new HealthComponent
        {
            Current = authoring.StatsSO.StartingHealth,
            Max = authoring.StatsSO.MaxHealth
        });

        AddComponentObject(entity, new EntityGameObjectLink
        {
            GameObject = authoring.gameObject
        });
    }
}
using UnityEngine;
using Unity.Entities;

public class CombatantStatsAuthoring : MonoBehaviour
{
    public CombatantStatsSO StatsSO;
}

class CombatantStatsBaker : Baker<CombatantStatsAuthoring>
{ 
public override void Bake(CombatantStatsAuthoring authoring)
    {
        if(authoring.StatsSO == null)
        {
            Debug.LogError($"{authoring.gameObject.name}: No StatsSO assigned.");
            return;
        }

        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new HealthComponent
        {
            Current = authoring.StatsSO.StartingHealth,
            Max = authoring.StatsSO.MaxHealth,
        });

        if (authoring.StatsSO is CombatantStatsSO combatant)
        {
            AddComponent(entity, new ShieldComponent
            {
                Current = authoring.StatsSO.StartingShield,
                Max = authoring.StatsSO.MaxShield,
            });

            AddComponent(entity, new DefenseComponent
            {
                Current = authoring.StatsSO.BaseDefense
            });

            AddComponent(entity, new SpeedComponent
            {
                Current = authoring.StatsSO.BaseSpeed,
                Max = authoring.StatsSO.MaxSpeed,
            });

            AddComponentObject(entity, new EntityGameObjectLink
            {
                GameObject = authoring.gameObject
            });
        }
    }
}



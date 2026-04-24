using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player")]
public class PlayerStatsSO : CombatantStatsSO
{
    [Header("Stamina")]
    public float MaxStamina;
    public float StartingStamina;

    [Header("Progression")]
    public int StartingLevel;
    public int MaxLevel;
    public float StartingExperience;
    public float ExperienceToLevelUp;
}

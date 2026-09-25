public enum AbilityTrigger { OnCooldown, OnSelfDied, OnAllyDied, OnAllyHealed }

[System.Serializable]
public class AbilityEntry {
    public AbilityTrigger Trigger;
    public AbilityData Ability;
}
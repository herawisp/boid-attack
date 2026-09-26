using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Ally Cooldown Buff")]
public class AllyCooldownBuffAbility : AbilityData {

    public float Multiplier = 0.5f;
    public float Duration = 5f;

    public override void Activate(Butterfly self) {
        Debug.Log("Activated");
        foreach (Butterfly ally in ButterflyService.Instance.GetButterflies(self.TeamType)) {
            if (ally == null) continue;
            ally.ApplyCooldownMultiplier(Multiplier, Duration);
        }
    }
}
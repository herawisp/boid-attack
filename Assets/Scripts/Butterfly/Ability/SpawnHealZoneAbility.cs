using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Spawn Heal Zone")]
public class SpawnHealZoneAbility : AbilityData {

    public Zone ZonePrefab;
    public float Duration = 15f;
    public float HealPerTick = 1f;
    public float TickInterval = 1f;

    public override void Activate(Butterfly self) {
        Zone zone = Instantiate(ZonePrefab, self.transform.position, Quaternion.identity);
        zone.Initialize(self.TeamType, Duration, false, onEnter: null, onExit: null);
        zone.SetTick(TickInterval, b => b.Heal(HealPerTick));
    }
}
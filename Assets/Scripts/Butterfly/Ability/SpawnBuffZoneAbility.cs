using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Spawn Buff Zone")]
public class SpawnBuffZoneAbility : AbilityData {

    public Zone ZonePrefab;
    public float Duration = 15f;

    public override void Activate(Butterfly self) {
        Zone zone = Instantiate(ZonePrefab, self.transform.position, Quaternion.identity);
        zone.Initialize(
            self.TeamType,
            Duration,
            false,
            onEnter: b => b.SetBuffed(true),
            onExit:  b => b.SetBuffed(false)
        );
    }
}
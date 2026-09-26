using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Attached Damage Aura")]
public class AttachedDamageAuraAbility : AbilityData {

    public Zone ZonePrefab;
    public float DamagePerTick = 0.5f;
    public float TickInterval = 0.1f;

    Zone _spawned;

    public override void Activate(Butterfly self) {
        if (self.HasAttachedAura) return;
        self.HasAttachedAura = true;

        Zone spawned = Instantiate(ZonePrefab, self.transform.position, Quaternion.identity, self.transform);
        spawned.transform.localPosition = Vector3.zero;
        spawned.Initialize(self.TeamType, duration: null, targetsOpposingTeam: true, onEnter: null, onExit: null);
        spawned.SetTick(TickInterval, b => b.TakeDamage(DamagePerTick));
    }
}
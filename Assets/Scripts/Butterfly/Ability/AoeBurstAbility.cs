using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/AOE Burst")]
public class AoeBurstAbility : AbilityData {

    public enum TargetMode { EnemiesOnly, AllButterflies }

    public float Radius = 2f;
    public float Damage = 15f;
    public TargetMode Targeting = TargetMode.EnemiesOnly;
    public AnimatedEffect VfxPrefab;

    public override void Activate(Butterfly self) {
        Vector3 center = GetCenter(self);

        foreach (Butterfly target in GetTargets(self)) {
            if (target == null) continue;
            if (Vector3.Distance(target.transform.position, center) > Radius) continue;

            target.TakeDamage(Damage);
        }

        if (VfxPrefab != null)
            Instantiate(VfxPrefab, center, Quaternion.identity);
    }

    IEnumerable<Butterfly> GetTargets(Butterfly self) {
        if (Targeting == TargetMode.EnemiesOnly) {
            TeamType enemyTeam = self.TeamType == TeamType.Player ? TeamType.Enemy : TeamType.Player;
            return ButterflyService.Instance.GetButterflies(enemyTeam);
        }

        return ButterflyService.Instance.GetButterflies(TeamType.Player)
            .Concat(ButterflyService.Instance.GetButterflies(TeamType.Enemy));
    }

    Vector3 GetCenter(Butterfly self) {
        Butterfly nearest = self.Vision.GetNearestOpposingButterfly(self.TeamType, self.transform.position);
        return nearest != null ? nearest.transform.position : self.transform.position;
    }
}
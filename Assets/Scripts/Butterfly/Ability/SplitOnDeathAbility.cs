using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Split On Death")]
public class SplitOnDeathAbility : AbilityData {

    public ButterflyType ButterflyType;
    public float MinHealthToSplit = 25f;
    public float Offset = 0.5f;

    public override void Activate(Butterfly self) {
        if (self.MaxHealth <= MinHealthToSplit) return;

        float childHealth = self.MaxHealth * 0.5f;
        float childAttack = self.ButterflyData.AttackDamage * 0.5f;

        SpawnChild(self, Vector3.left * Offset, childHealth, childAttack);
        SpawnChild(self, Vector3.right * Offset, childHealth, childAttack);
    }

    void SpawnChild(Butterfly self, Vector3 offset, float health, float attack) {
        float childScale = self.transform.localScale.x * 0.5f;

        ButterflyService.Instance.SpawnButterfly(
            ButterflyType, self.TeamType, self.transform.position + offset,
            scale: childScale, healthOverride: health, attackOverride: attack
        );
    }
}
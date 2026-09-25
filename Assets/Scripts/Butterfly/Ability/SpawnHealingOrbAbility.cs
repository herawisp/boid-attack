using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Spawn Healing Orb")]
public class SpawnHealingOrbAbility : AbilityData {

    public HealingOrb OrbPrefab;
    public float HealAmount = 10f;

    public override void Activate(Butterfly self) {
        HealingOrb orb = Instantiate(OrbPrefab, self.transform.position, Quaternion.identity);
        orb.TeamType = self.TeamType;
        orb.HealAmount = HealAmount;
        orb.SetTypeEnemy();
    }
}
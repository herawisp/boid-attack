using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Spawn Butterfly")]
public class SpawnButterflyAbility : AbilityData {

    public ButterflyType ButterflyType;
    public float Scale = 1f;

    public override void Activate(Butterfly self) {
        ButterflyService.Instance.SpawnButterfly(ButterflyType, self.TeamType, self.transform.position, Scale);
    }
}
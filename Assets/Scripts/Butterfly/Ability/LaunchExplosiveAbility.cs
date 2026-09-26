using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Launch Explosive")]
public class LaunchExplosiveAbility : AbilityData {

    public Explosive ExplosivePrefab;
    public float Speed = 6f;
    public float ExplosionRadius = 2f;
    public float ExplosionDamage = 30f;

    public override void Activate(Butterfly self) {
        Butterfly target = self.Vision.GetNearestOpposingButterfly(self.TeamType, self.transform.position);
        if (target == null) return;
        Vector3 direction = (target.transform.position - self.transform.position).normalized;
        Debug.Log($"Explosive direction: {direction}, self: {self.transform.position}, target: {target.transform.position}");

        Explosive explosive = Instantiate(ExplosivePrefab, self.transform.position, Quaternion.identity);
        explosive.TeamType = self.TeamType;
        explosive.Speed = Speed;
        explosive.ExplosionRadius = ExplosionRadius;
        explosive.ExplosionDamage = ExplosionDamage;
        explosive.Launch(self.transform.position, direction);
    }
}
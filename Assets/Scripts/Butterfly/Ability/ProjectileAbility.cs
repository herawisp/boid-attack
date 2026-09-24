using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Fire Projectiles")]
public class ProjectileAbility : AbilityData {

    //================================================================================================//
    //================================================================================================//

    public float[] AngleOffsets = { 0f };
    public int ProjectileAmount = 1;
    public float DelayBetweenShots = 0.5f;
    public bool Homing;
    public bool Empowered;

    public Bullet BulletPrefab;

    //================================================================================================//
    //================================================================================================//

    public override void Activate(Butterfly self) {
        self.StartCoroutine(FireRoutine(self));
    }
    
    //================================================================================================//
    //================================================================================================//

    IEnumerator FireRoutine(Butterfly self) {
        for (int i = 0; i < ProjectileAmount; i++) {
            if (self == null) yield break;

            Butterfly target = self.Vision.GetNearestOpposingButterfly(self.TeamType, self.transform.position);
            if (target == null) yield break;

            Vector3 targetVelocity = target.GetComponent<SteeringBehaviour>().Velocity;
            Vector3 aimDirection = AimUtils.ComputeAimDirection(
                self.transform.position,
                target.transform.position,
                targetVelocity,
                10
            );

            foreach (float angle in AngleOffsets) {
                Spawn(self, Rotate(aimDirection, angle));
            }
            if (i < ProjectileAmount - 1)
                yield return new WaitForSeconds(DelayBetweenShots);
        }
    }

    void Spawn(Butterfly self, Vector3 direction) {
        Bullet bullet = Instantiate(BulletPrefab, self.transform.position, Quaternion.identity);
        bullet.AttackDamage = self.ButterflyData.AttackDamage;
        bullet.TeamType = self.TeamType;
        bullet.Empowered = Empowered;
        bullet.Shoot(self.transform.position, direction);
    }

    static Vector3 Rotate(Vector3 dir, float degrees) {
        return Quaternion.AngleAxis(degrees, Vector3.forward) * dir;
    }

    //================================================================================================//
    //================================================================================================//

}

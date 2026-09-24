using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Butterfly/Abilities/Fire Projectiles")]
public class ProjectileAbility : AbilityData {

    //================================================================================================//
    //================================================================================================//

    public float[] AngleOffsets;
    public int ProjectileAmount;
    public float DelayBetweenShots = 0.5f;
    public bool Homing;

    public Bullet BulletPrefab;

    //================================================================================================//
    //================================================================================================//

    public override void Activate(Butterfly self) {
        Butterfly target = self.Vision.GetNearestOpposingButterfly(self.TeamType, self.transform.position);
        if (target == null) return;

        Vector3 targetVelocity = target.GetComponent<SteeringBehaviour>().Velocity;
        Vector3 aimDirection = AimUtils.ComputeAimDirection(
            self.transform.position,
            target.transform.position,
            targetVelocity,
            10
        );

        if (DelayBetweenShots <= 0f) {
            foreach (float angle in AngleOffsets) 
                Spawn(self, Rotate(aimDirection, angle));
        } else {
            self.StartCoroutine(FireStaggered(self, aimDirection));
        }
    }
    
    //================================================================================================//
    //================================================================================================//

    IEnumerator FireStaggered(Butterfly self, Vector3 aimDirection) {
        foreach (float angle in AngleOffsets) {
            if (self == null) yield break;
            Spawn(self, Rotate(aimDirection, angle));
            yield return new WaitForSeconds(DelayBetweenShots);
        }
    }

    void Spawn(Butterfly self, Vector3 direction) {
        Bullet bullet = Instantiate(BulletPrefab, self.transform.position, Quaternion.identity);
        bullet.AttackDamage = self.ButterflyData.AttackDamage;
        bullet.TeamType = self.TeamType;
        bullet.Shoot(self.transform.position, direction);
    }

    static Vector3 Rotate(Vector3 dir, float degrees) {
        return Quaternion.AngleAxis(degrees, Vector3.forward) * dir;
    }

    //================================================================================================//
    //================================================================================================//

}

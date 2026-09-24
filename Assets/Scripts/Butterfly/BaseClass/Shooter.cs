using System.Collections;
using System.Threading;
using UnityEngine;

public class Shooter: MonoBehaviour {

    //================================================================================================//
    //================================================================================================//
    
    public Bullet BulletPrefab;
    public int ProjectileAmount = 3;

    private Vision _vision;
    private Butterfly _butterfly;
    private Timer _cooldownTimer;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _vision = GetComponent<Vision>();
        _butterfly = GetComponent<Butterfly>();
    }

    void Start() {
        _cooldownTimer = gameObject.AddComponent<Timer>();
        _cooldownTimer.Paused = false;
        _cooldownTimer.Timeout.AddListener(OnCooldownTimerTimeout);
    }

    //================================================================================================//
    //================================================================================================//

    void Shoot()
    {
        StartCoroutine(OnShoot());
    }

    IEnumerator OnShoot() {
        for (int i = 0; i < ProjectileAmount; i++) {
            Butterfly nearestButterfly = _vision.GetNearestOpposingButterfly(_butterfly.TeamType, _butterfly.transform.position);
            if (nearestButterfly == null) continue;

            Bullet bullet = Instantiate(BulletPrefab, transform.position, new());
            bullet.name = _butterfly.TeamType.ToString();
            
            Vector3 interceptPoint;
            SteeringBehaviour steeringBehaviour = nearestButterfly.gameObject.GetComponent<SteeringBehaviour>();
            TryGetInterceptPoint(transform.position, nearestButterfly.transform.position, steeringBehaviour.Velocity, 10, out interceptPoint);

            Vector3 direction = (interceptPoint - transform.position).normalized;
            bullet.Shoot(transform.position, direction);
            yield return new WaitForSeconds(1f);
        }
    }

    void OnCooldownTimerTimeout() {
        Shoot();
    }

    public static bool TryGetInterceptPoint(
        Vector3 shooterPos, 
        Vector3 targetPos, 
        Vector3 targetVelocity, 
        float bulletSpeed, 
        out Vector3 interceptPoint
    ) {
        Vector3 toTarget = targetPos - shooterPos;

        float a = Vector3.Dot(targetVelocity, targetVelocity) - bulletSpeed * bulletSpeed;
        float b = 2f * Vector3.Dot(targetVelocity, toTarget);
        float c = Vector3.Dot(toTarget, toTarget);

        float t;

        if (Mathf.Abs(a) < 0.0001f) {
            if (Mathf.Abs(b) < 0.0001f) {
                interceptPoint = targetPos;
                return false;
            }
            t = -c / b;
        } else {
            float discriminant = b * b - 4f * a * c;
            if (discriminant < 0f) {
                interceptPoint = targetPos;
                return false;
            }

            float sqrtDisc = Mathf.Sqrt(discriminant);
            float t1 = (-b + sqrtDisc) / (2f * a);
            float t2 = (-b - sqrtDisc) / (2f * a);

            t = Mathf.Min(t1, t2) > 0 ? Mathf.Min(t1, t2) : Mathf.Max(t1, t2);
            if (t < 0) {
                interceptPoint = targetPos;
                return false;
            }
        }

        interceptPoint = targetPos + targetVelocity * t;
        return true;
    }

    //================================================================================================//
    //================================================================================================//
}
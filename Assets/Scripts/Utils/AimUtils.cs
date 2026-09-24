using UnityEngine;

public static class AimUtils {
    
    public static Vector3 ComputeAimDirection(
        Vector3 shooterPos,
        Vector3 targetPos,
        Vector3 targetVelocity,
        float bulletSpeed)
    {
        if (TryGetInterceptPoint(shooterPos, targetPos, targetVelocity, bulletSpeed, out Vector3 intercept))
            return (intercept - shooterPos).normalized;

        return (targetPos - shooterPos).normalized;
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
}

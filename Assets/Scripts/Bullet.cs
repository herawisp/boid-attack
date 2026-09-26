using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    //================================================================================================//
    //================================================================================================//

    public float AttackDamage;
    public TeamType TeamType;
    public bool Empowered;
    public bool Homing;
    public float SpriteAngleOffset = -90f;
    public float TurnSpeed = 200f;
    public float Speed = 10f;

    Butterfly _target;
    Vector3 _direction;
    Animator _animator;
    Vision _vision;
    SpriteRenderer _spriteRenderer;
    float _currentAngle;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (Homing) {
            if (_target == null) AcquireTarget();

            if (_target != null) {
                Vector3 toTarget = _target.transform.position - transform.position;
                float targetAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
                _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, targetAngle, TurnSpeed * Time.deltaTime);
            }
        }

        Vector3 direction = new Vector3(Mathf.Cos(_currentAngle * Mathf.Deg2Rad), Mathf.Sin(_currentAngle * Mathf.Deg2Rad), 0f);
        transform.position += direction * Speed * Time.deltaTime;
        _direction = direction;
        LookAtMovingDirection();
    }

    //================================================================================================//
    //================================================================================================//

    public void Shoot(Vector3 origin, Vector3 direction, Vision vision = null) {
        transform.position = origin;
        direction.Normalize();
        _currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _vision = vision;

        if (Homing) AcquireTarget();
    }

    void AcquireTarget() {
        if (_vision == null) return;
        _target = _vision.GetNearestOpposingButterfly(TeamType, transform.position);
    }

    public void SetAnimationController(RuntimeAnimatorController animatorController) {
        _animator.runtimeAnimatorController = animatorController;
    }

    public void SetTypeEnemy() {
        _spriteRenderer.color = new(1, 0.5f, 0.5f);
    }

    void LookAtMovingDirection() {
        if (_direction.sqrMagnitude == 0.0f) return;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //================================================================================================//
    //================================================================================================//
}

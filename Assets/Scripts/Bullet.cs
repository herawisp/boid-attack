using UnityEngine;

public class Bullet : MonoBehaviour {
    
    //================================================================================================//
    //================================================================================================//

    public float AttackDamage;
    public TeamType TeamType;
    public bool Empowered;

    Vector3 _direction;
    Rigidbody2D _rigidBody;
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        _rigidBody.linearVelocity = _direction;
        if (ScreenUtils.IsOutsideBorder(transform.position)) {
            Destroy(gameObject);
        }
    }

    //================================================================================================//
    //================================================================================================//

    public void Shoot(Vector3 position, Vector3 direction) {
        _direction = direction * 10;
        transform.position = position;
        LookAtMovingDirection();
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

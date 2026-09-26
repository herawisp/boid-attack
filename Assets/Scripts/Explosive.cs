using UnityEngine;

public class Explosive : MonoBehaviour {

    public float Speed = 6f;
    public float ExplosionRadius = 2f;
    public float ExplosionDamage = 30f;
    public float SpriteAngleOffset = -90f;
    public TeamType TeamType;
    public AnimatedEffect VfxPrefab;

    Vector3 _direction;

    public void Launch(Vector3 origin, Vector3 direction) {
        transform.position = origin;
        _direction = direction.normalized;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + SpriteAngleOffset);
    }

    void Update() {
        transform.position += _direction * Speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.TryGetComponent(out Butterfly butterfly)) return;
        if (butterfly.TeamType == TeamType) return;

        Explode();
    }

    void Explode() {
        TeamType enemyTeam = TeamType == TeamType.Player ? TeamType.Enemy : TeamType.Player;

        foreach (Butterfly target in ButterflyService.Instance.GetButterflies(enemyTeam)) {
            if (target == null) continue;
            if (Vector3.Distance(target.transform.position, transform.position) > ExplosionRadius) continue;

            target.TakeDamage(ExplosionDamage);
        }

        if (VfxPrefab != null)
            Instantiate(VfxPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
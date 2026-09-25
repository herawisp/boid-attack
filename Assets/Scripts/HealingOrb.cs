using UnityEngine;

public class HealingOrb : MonoBehaviour {

    public float HealAmount = 10f;
    public TeamType TeamType; 
    public float Lifetime = 8f; 

    private SpriteRenderer _spriteRenderer;

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        Destroy(gameObject, Lifetime);
    }

    public void SetTypeEnemy() {
        _spriteRenderer.color = new(1, 0.5f, 0.5f);
    }
    
    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.TryGetComponent(out Butterfly butterfly)) return;
        if (butterfly.TeamType != TeamType) return;
        if (butterfly.Health >= butterfly.MaxHealth) return;

        butterfly.Heal(HealAmount);
        Destroy(gameObject);
    }
}
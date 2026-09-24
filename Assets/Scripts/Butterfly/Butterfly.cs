using UnityEngine;


public class Butterfly : MonoBehaviour { 

    //================================================================================================//
    //================================================================================================//

    [Header("Butterfly Stats")]
    public float Health;
    public float MaxHealth;

    [Header("Butterfly Datas")]
    public ButterflyData ButterflyData;
    public TeamType TeamType;
    public Vision Vision;

    private Timer _timer;
    private Movement _movement;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _movement = gameObject.AddComponent<Movement>();
        _movement.UpdateFlockingAgents(ButterflyService.Instance.SteeringBehaviours);
        _movement.TeamType = TeamType;
        _movement.Paused = true;

        _timer = gameObject.AddComponent<Timer>();
        _timer.SetWaitTime(ButterflyData.Cooldown);  

        Vision = gameObject.AddComponent<Vision>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Bullet bullet;
        if (!collision.TryGetComponent(out bullet)) return;
        if (collision.gameObject.name == TeamType.ToString()) return;

        Health -= bullet.AttackDamage;
        Destroy(collision.gameObject);

        if (Health > 0) return;
        ButterflyService.Instance.RemoveButterfly(this, TeamType);
    }

    //================================================================================================//
    //================================================================================================//

    public void Enable() {
        Health = ButterflyData.Health;
        MaxHealth = ButterflyData.Health;
        _movement.Paused = false;
    }

    public void Disable() {
        _movement.Paused = true;
    }
    
    //================================================================================================//
    //================================================================================================//
}

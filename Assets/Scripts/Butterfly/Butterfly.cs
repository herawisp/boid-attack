using System.Collections.Generic;
using System.Linq;
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
    public bool Enabled = false;

    private Timer _timer;
    private Movement _movement;

    //================================================================================================//
    //================================================================================================//

    public void Initialize(ButterflyData butterflyData, TeamType teamType) {
        ButterflyData = butterflyData;
        TeamType = teamType;

        _movement = gameObject.AddComponent<Movement>();
        _movement.UpdateFlockingAgents(ButterflyService.Instance.SteeringBehaviours);
        _movement.TeamType = TeamType;
        _movement.Paused = true;

        _timer = gameObject.AddComponent<Timer>();
        _timer.SetWaitTime(ButterflyData.Cooldown);  
        _timer.Timeout.AddListener(OnCooldown);

        Vision = gameObject.AddComponent<Vision>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Bullet bullet;
        if (!collision.TryGetComponent(out bullet)) return;
        if (bullet.TeamType == TeamType) return;

        if (bullet.Empowered) {
            Debug.Log(ButterflyService.Instance.CountEnabledButterfly());
            Health -= 2 * ButterflyService.Instance.CountEnabledButterfly();
        } else Health -= bullet.AttackDamage;

        Destroy(collision.gameObject);

        if (Health > 0) return;
        Disable();
    }

    void OnCooldown() {
        foreach (AbilityData abilityData in ButterflyData.Abilities) {
            abilityData.Activate(this);
        }
    }

    //================================================================================================//
    //================================================================================================//

    public void Enable() {
        Health = ButterflyData.Health;
        MaxHealth = ButterflyData.Health;
        Enabled = true;
        gameObject.SetActive(true);
        _movement.Paused = false;
        _timer.Paused = false;
    }

    public void Disable() {
        Enabled = false;
        gameObject.SetActive(false);
        _movement.Paused = true;
        _timer.Paused = true;
    }
    
    //================================================================================================//
    //================================================================================================//
}

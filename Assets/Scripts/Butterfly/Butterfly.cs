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

    public void Heal(float amount) {
        Health = Mathf.Min(Health + amount, MaxHealth);
        ButterflyService.Instance.RaiseButterflyHealed(this);
    }

    //================================================================================================//
    //================================================================================================//

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
        ButterflyService.Instance.RaiseButterflyDied(this);
        Disable();
    }

    void Trigger(AbilityTrigger trigger) {
        foreach (AbilityEntry entry in ButterflyData.Abilities) {
            if (entry.Trigger == trigger)
                entry.Ability.Activate(this);
        }
    }

    public void Enable() {
        Health = ButterflyData.Health;
        MaxHealth = ButterflyData.Health;
        _movement.Paused = false;
        _timer.Paused = false;
        gameObject.SetActive(true);
        Enabled = true;

        ButterflyService.Instance.ButterflyDied += OnAnyButterflyDied;
        ButterflyService.Instance.ButterflyHealed += OnAnyButterflyHealed;
    }

    public void Disable() {
        _movement.Paused = true;
        _timer.Paused = true;
        gameObject.SetActive(false);
        Enabled = false;

        ButterflyService.Instance.ButterflyDied -= OnAnyButterflyDied;
        ButterflyService.Instance.ButterflyHealed -= OnAnyButterflyHealed;
    }

    void OnAnyButterflyDied(Butterfly dead) {
    if (dead == this)
        Trigger(AbilityTrigger.OnSelfDied);
    else if (dead.TeamType == TeamType && Health > 0)
        Trigger(AbilityTrigger.OnAllyDied);
    }

    void OnAnyButterflyHealed(Butterfly healed) {
        if (healed != this && healed.TeamType == TeamType)
            Trigger(AbilityTrigger.OnAllyHealed);
    }

    void OnCooldown() => Trigger(AbilityTrigger.OnCooldown);
    
    //================================================================================================//
    //================================================================================================//
}

using System.Collections;
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
    public bool IsSpawned;
    public bool HasAttachedAura;

    public float CooldownMultiplier = 1f;
    public float AttackMultiplier = 1f;
    public float? AttackOverride;

    public float CurrentCooldown => ButterflyData.Cooldown * CooldownMultiplier;
    public float CurrentAttackDamage => (AttackOverride ?? ButterflyData.AttackDamage) * AttackMultiplier;

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

    public void SetBuffed(bool buffed) {
        Debug.Log("Buffed");
        CooldownMultiplier = buffed ? 0.5f : 1f;
        AttackMultiplier = buffed ? 2f : 1f;
    }

    public void TakeDamage(float amount) {
        Health -= amount;
        if (Health > 0) return;

        ButterflyService.Instance.RaiseButterflyDied(this);
        Disable();
    }

    public void TakeDamage(Bullet bullet) {
        float amount = bullet.Empowered
            ? 2 * ButterflyService.Instance.CountEnabledButterfly()
            : bullet.AttackDamage;

        TakeDamage(amount);
    }

    //================================================================================================//
    //================================================================================================//

    public void ApplyCooldownMultiplier(float multiplier, float duration) {
        StartCoroutine(CooldownMultiplierRoutine(multiplier, duration));
    }

    IEnumerator CooldownMultiplierRoutine(float multiplier, float duration) {
        CooldownMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        if (this == null) yield break;
        CooldownMultiplier = 1f;
    }

    //================================================================================================//
    //================================================================================================//

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.TryGetComponent(out Bullet bullet)) return;
        if (bullet.TeamType == TeamType) return;

        TakeDamage(bullet);
        Destroy(collision.gameObject);
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

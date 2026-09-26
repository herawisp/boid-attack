using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public TeamType TeamType;
    public float Duration = 15f;
    public bool TargetsOpposingTeam;

    System.Action<Butterfly> _onEnter;
    System.Action<Butterfly> _onExit;
    System.Action<Butterfly> _onTick;
    float _tickInterval;
    float _tickTimer;

    SpriteRenderer _spriteRenderer;
    List<Butterfly> _inside = new List<Butterfly>();

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    public void Initialize(TeamType team, float? duration, bool targetsOpposingTeam, System.Action<Butterfly> onEnter, System.Action<Butterfly> onExit) {
        TeamType = team;
        TargetsOpposingTeam = targetsOpposingTeam;
        _onEnter = onEnter;
        _onExit = onExit;

        if (duration.HasValue)
            Destroy(gameObject, duration.Value);
    }

    bool IsValidTarget(Butterfly butterfly) {
        bool sameTeam = butterfly.TeamType == TeamType;
        return TargetsOpposingTeam ? !sameTeam : sameTeam;
    }

    public void SetTick(float interval, System.Action<Butterfly> onTick) {
        _tickInterval = interval;
        _onTick = onTick;
    }

    void Update() {
        if (_onTick == null) return;

        _tickTimer += Time.deltaTime;
        if (_tickTimer < _tickInterval) return;
        _tickTimer = 0f;

        foreach (Butterfly b in _inside) {
            if (b != null) _onTick(b);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.TryGetComponent(out Butterfly butterfly)) return;
        if (!IsValidTarget(butterfly)) return;

        _inside.Add(butterfly);
        _onEnter?.Invoke(butterfly);
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.TryGetComponent(out Butterfly butterfly)) return;
        if (!_inside.Remove(butterfly)) return;

        _onExit?.Invoke(butterfly);
    }

    void OnDestroy() {
        foreach (Butterfly b in _inside) {
            if (b != null) _onExit?.Invoke(b);
        }
    }
}
using UnityEngine;

public class Shooter: MonoBehaviour {

    Timer _cooldownTimer;

    public void Start()
    {
        _cooldownTimer = gameObject.AddComponent<Timer>();
        _cooldownTimer.Paused = false;
        _cooldownTimer.Timeout.AddListener(OnCooldownTimerTimeout);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCooldownTimerTimeout()
    {
        Debug.Log("Timeout");
    }
}

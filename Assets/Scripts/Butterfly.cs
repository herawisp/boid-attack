using UnityEngine;


public class Butterfly : MonoBehaviour { 

    //================================================================================================//
    //================================================================================================//

    public ServiceManager ServiceManager;
    public TeamType TeamType;

    Movement _movement;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _movement = gameObject.AddComponent<Movement>();
        _movement.ButterflyTransform = transform;
        _movement.Paused = true;
    }

    public void Enable() {
        _movement.UpdateFlockingAgents(ServiceManager.ButterflyService.SteeringBehaviours);
        _movement.TeamType = TeamType;
        _movement.Paused = false;
    }

    public void Disable() {
        _movement.Paused = true;
    }
    
    //================================================================================================//
    //================================================================================================//
}

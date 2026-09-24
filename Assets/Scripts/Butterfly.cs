using UnityEngine;


public class Butterfly : MonoBehaviour { 

    //================================================================================================//
    //================================================================================================//

    public ButterflyType butterflyType;
    public TeamType TeamType;


    public int Health;
    public int MaxHealth;

    Movement _movement;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _movement = gameObject.AddComponent<Movement>();
        _movement.ButterflyTransform = transform;
        _movement.Paused = true;
    }

    public void Enable() {
        _movement.UpdateFlockingAgents(ButterflyService.Instance.SteeringBehaviours);
        _movement.TeamType = TeamType;
        _movement.Paused = false;

        ButterflyData butterflyData = ButterflyService.Instance.ButterflyDatas[(int) butterflyType];
        Health = butterflyData.Health;
        MaxHealth = butterflyData.Health;
    }

    public void Disable() {
        _movement.Paused = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == TeamType.ToString()) return;
        Destroy(collision.gameObject);
        Health--;

        if (Health == 0)
        {
            ButterflyService.Instance.RemoveButterfly(this, TeamType);
        }
    }
    
    //================================================================================================//
    //================================================================================================//
}

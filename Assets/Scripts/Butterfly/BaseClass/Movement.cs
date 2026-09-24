using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour {
    
    //================================================================================================//
    //================================================================================================//

    public TeamType TeamType;
    public bool Paused;

    private SteeringBehaviour _steeringBehaviour;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _steeringBehaviour = gameObject.AddComponent<SteeringBehaviour>();
    }

    void Update() {
        if (Paused == true) return;
        if (TeamType == TeamType.Player) {
            UpdatePlayerMovement();  
        } else {
            UpdateEnemyMovement();
        }
    }

    void Start() {
        if (TeamType == TeamType.Enemy) {
            _steeringBehaviour.WanderDistance = 5f;
            _steeringBehaviour.WanderPower = 3f;
            _steeringBehaviour.WanderChange = 7f;        
        }
    }

    //================================================================================================//
    //================================================================================================//

    public void UpdateFlockingAgents(List<SteeringBehaviour> steeringBehaviours) {
        _steeringBehaviour.AgentFlockList = steeringBehaviours;
    }

    //================================================================================================//
    //================================================================================================//

    void UpdatePlayerMovement() {
        _steeringBehaviour.SeekTargetPosition = GetMouseSeekPosition();
        transform.position = _steeringBehaviour.Position;
        LookAtMovingDirection();

        bool isOutsideBorder = ScreenUtils.IsOutsideBorder(transform.position);
        if (isOutsideBorder) {
            _steeringBehaviour.SeekForceWeight = 1f;
            _steeringBehaviour.FleeForceWeight = 0;
        } else {
            (Vector3 borderPosition, float distance) = ScreenUtils.GetClosestDistanceToBorder(transform.position);
            if (distance <= 3) {
                _steeringBehaviour.WanderForceWeight = 0f;
                _steeringBehaviour.SeekForceWeight = 0f;
                _steeringBehaviour.FleeForceWeight = 1f;
                _steeringBehaviour.FleeTargetPosition = borderPosition;
            } else {
                _steeringBehaviour.WanderForceWeight = 1f;
                _steeringBehaviour.SeekForceWeight = 1f;
                _steeringBehaviour.FleeForceWeight = 0;
            }
        }
    }

    void UpdateEnemyMovement() {
        transform.position = _steeringBehaviour.Position;
        LookAtMovingDirection();

        bool isOutsideBorder = ScreenUtils.IsOutsideBorder(transform.position);
        if (isOutsideBorder) {
            _steeringBehaviour.SeekTargetPosition = ScreenUtils.GetMiddleWorldPosition(transform.position);
            _steeringBehaviour.SeekForceWeight = 1f;
            _steeringBehaviour.FleeForceWeight = 0f;
        } else {
            (Vector3 borderPosition, float distance) = ScreenUtils.GetClosestDistanceToBorder(transform.position); 
            if (distance <= 3) {
                _steeringBehaviour.WanderForceWeight = 0f;
                _steeringBehaviour.SeekForceWeight = 0f;
                _steeringBehaviour.FleeForceWeight = 1f;
                _steeringBehaviour.FleeTargetPosition = borderPosition;
            } else {
                _steeringBehaviour.SeekForceWeight = 0f;
                _steeringBehaviour.FleeForceWeight = 0f;
                _steeringBehaviour.WanderForceWeight = 1f;
            }
        }   
    }

    //================================================================================================//
    //================================================================================================//

    void LookAtMovingDirection() {
        Vector2 velocity = _steeringBehaviour.Velocity;
        if (velocity.sqrMagnitude ==  0.0f) return;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    Vector2 GetMouseSeekPosition() {
        Vector3 mouseWorldPosition = ScreenUtils.GetMouseWorldPosition();
        bool isMouseOutsideBorder = ScreenUtils.IsOutsideBorder(mouseWorldPosition);
        if (isMouseOutsideBorder) mouseWorldPosition = ScreenUtils.GetMiddleWorldPosition(transform.position);
        return new(mouseWorldPosition.x, mouseWorldPosition.y);
    }

    //================================================================================================//
    //================================================================================================//
}

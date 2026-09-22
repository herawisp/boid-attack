using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SteeringBehaviour))]
[RequireComponent(typeof(SpriteRenderer))]

public class Butterfly : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    private SteeringBehaviour _steeringBehaviour;
    private SpriteRenderer _spriteRenderer;
    public TeamType TeamType = TeamType.Player;
    
    Shooter shooter;

    //================================================================================================//
    //================================================================================================//

    void Start()
    {
        shooter = gameObject.AddComponent<Shooter>();
    }
    void Awake() {
        _steeringBehaviour = GetComponent<SteeringBehaviour>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (TeamType == TeamType.Player) {
            _steeringBehaviour.SeekTargetPosition = UpdateTargetPosition();
            LookAtMovingDirection();
            transform.position = _steeringBehaviour.Position;

            bool isOutsideBorder = IsOutsideBorder(transform.position);
            if (isOutsideBorder) {
                _steeringBehaviour.SeekForceWeight = 1f;
                _steeringBehaviour.FleeForceWeight = 0;
            } else {
                ReactToClosestBorder();   
            }   
        } else {
            LookAtMovingDirection();
            transform.position = _steeringBehaviour.Position;

            bool isOutsideBorder = IsOutsideBorder(transform.position);
            if (isOutsideBorder) {
                _steeringBehaviour.SeekTargetPosition = GetMiddleWorldPosition();
                _steeringBehaviour.SeekForceWeight = 1f;
                _steeringBehaviour.FleeForceWeight = 0f;
            } else {
                ReactToClosestBorder();   
            }   
        }
    }

    //================================================================================================//
    //================================================================================================//
    
    void LookAtMovingDirection() {
        Vector2 velocity = _steeringBehaviour.Velocity;
        if (velocity.sqrMagnitude != 0.0f) {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    Vector2 UpdateTargetPosition() {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        if (IsOutsideBorder(worldPosition)) worldPosition = GetMiddleWorldPosition();
        return new(worldPosition.x, worldPosition.y);
    }


    //================================================================================================//
    //================================================================================================//

    Vector3 GetMiddleWorldPosition() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 middleScreen = new(Screen.width / 2, Screen.height / 2, screenPos.z);
        Vector3 middleWorld = Camera.main.ScreenToWorldPoint(middleScreen);
        return middleWorld;
    }

    List<Vector3> GetScreenBorderWorldPosition() {

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 topScreen = new(screenPos.x, Screen.height, screenPos.z);
        Vector3 bottomScreen = new(screenPos.x, 0f, screenPos.z);
        Vector3 leftScreen = new(0f, screenPos.y, screenPos.z);
        Vector3 rightScreen = new(Screen.width, screenPos.y, screenPos.z);

        Vector3 topWorld = Camera.main.ScreenToWorldPoint(topScreen);
        Vector3 bottomWorld = Camera.main.ScreenToWorldPoint(bottomScreen);
        Vector3 leftWorld = Camera.main.ScreenToWorldPoint(leftScreen);
        Vector3 rightWorld = Camera.main.ScreenToWorldPoint(rightScreen);

        return new List<Vector3>() { topWorld, bottomWorld, leftWorld, rightWorld };
    } 

    (Vector3, float) GetClosestDistanceToBorder() {

        List<Vector3> borderWorldPosition = GetScreenBorderWorldPosition();
        Vector3 closestBorderPosition = borderWorldPosition[0];
        float closestDistance = Mathf.Infinity;

        foreach (Vector3 position in borderWorldPosition) {
            float distance = Vector3.Distance(position, transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestBorderPosition = position;
            }
        }

        return (closestBorderPosition, closestDistance);
    }

    bool IsOutsideBorder(Vector3 position) {
        List<Vector3> borderWorldPosition = GetScreenBorderWorldPosition();
        if (position.y > borderWorldPosition[0].y) return true;
        if (position.y < borderWorldPosition[1].y) return true;
        if (position.x < borderWorldPosition[2].x) return true;
        if (position.x > borderWorldPosition[3].x) return true;
        return false;
    }

    void ReactToClosestBorder() {
        Vector3 borderPosition;
        float distance;
        (borderPosition, distance) = GetClosestDistanceToBorder();

        if (TeamType == TeamType.Player) {
            if (distance <= 3) {
                _steeringBehaviour.SeekForceWeight = 0.2f;
                _steeringBehaviour.FleeForceWeight = 1f;
                _steeringBehaviour.FleeTargetPosition = borderPosition;
            } else {
                _steeringBehaviour.SeekForceWeight = 1f;
                _steeringBehaviour.FleeForceWeight = 0;
            }
        } else {
            if (distance <= 3) {
                _steeringBehaviour.WanderForceWeight = 0f;
                _steeringBehaviour.SeekForceWeight = 0f;
                _steeringBehaviour.FleeForceWeight = 1f;
                _steeringBehaviour.FleeTargetPosition = borderPosition;
            } else {
                _steeringBehaviour.SeekForceWeight = 0f;
                _steeringBehaviour.FleeForceWeight = 0f;
                _steeringBehaviour.WanderForceWeight = 1f;

                _steeringBehaviour.WanderDistance = 5f;
                _steeringBehaviour.WanderPower = 3f;
                _steeringBehaviour.WanderChange = 7f;
            }
        }
    }
    
    //================================================================================================//
    //================================================================================================//
}

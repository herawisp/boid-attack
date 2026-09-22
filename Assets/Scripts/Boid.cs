using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(SteeringBehaviour))]
[RequireComponent(typeof(SpriteRenderer))]

public class Boid : MonoBehaviour
{

    private SteeringBehaviour _steeringBehaviour;
    private SpriteRenderer _spriteRenderer;

    void Awake() {
        _steeringBehaviour = GetComponent<SteeringBehaviour>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        UpdateSeekTargetPosition();
        LookAtMovingDirection();
        transform.position = _steeringBehaviour.Position;
        ReactToClosestBorder();
    }

    void LookAtMovingDirection() {
        Vector2 velocity = _steeringBehaviour.Velocity;
        if (velocity.sqrMagnitude != 0.0f) {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void UpdateSeekTargetPosition() {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        _steeringBehaviour.SeekTargetPosition = new(worldPosition.x, worldPosition.y);
    }

    //================================================================================================//
    //================================================================================================//

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

    void ReactToClosestBorder() {
        Vector3 borderPosition;
        float distance;
        (borderPosition, distance) = GetClosestDistanceToBorder();

        Debug.Log(distance);
        if (distance <= 3) {
            Debug.Log("FLEEING");
            _steeringBehaviour.SeekForceWeight = 0.2f;
            _steeringBehaviour.FleeForceWeight = 1f;
            _steeringBehaviour.FleeTargetPosition = borderPosition;
        } else {
            Debug.Log("SEEKING");
            _steeringBehaviour.SeekForceWeight = 1f;
            _steeringBehaviour.FleeForceWeight = 0;
        }
    }
    
    //================================================================================================//
    //================================================================================================//
}

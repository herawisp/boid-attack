using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ScreenUtils {

    public static Vector3 GetMouseWorldPosition() {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    public static Vector3 GetMiddleWorldPosition(Vector3 position) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        Vector3 middleScreen = new(Screen.width / 2, Screen.height / 2, screenPos.z);
        return Camera.main.ScreenToWorldPoint(middleScreen);
    }
    
    public static Vector3 GetTopWorldPosition(Vector3 position) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        Vector3 topScreen = new(screenPos.x, Screen.height, screenPos.z);
        return Camera.main.ScreenToWorldPoint(topScreen);
    }

    public static Vector3 GetBottomWorldPosition(Vector3 position) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        Vector3 bottomScreen = new(screenPos.x, 0f, screenPos.z);
        return Camera.main.ScreenToWorldPoint(bottomScreen);
    }

    public static Vector3 GetRightWorldPosition(Vector3 position) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        Vector3 rightScreen = new(Screen.width, screenPos.y, screenPos.z);
        return Camera.main.ScreenToWorldPoint(rightScreen);
    }

    public static Vector3 GetLeftWorldPosition(Vector3 position) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        Vector3 leftScreen = new(0f, screenPos.y, screenPos.z);
        return Camera.main.ScreenToWorldPoint(leftScreen);
    }

    public static List<Vector3> GetScreenBorderWorldPosition(Vector3 position) {
        Vector3 topWorld = GetTopWorldPosition(position);
        Vector3 bottomWorld = GetBottomWorldPosition(position);
        Vector3 leftWorld = GetLeftWorldPosition(position);
        Vector3 rightWorld = GetRightWorldPosition(position);
        return new List<Vector3>() { topWorld, bottomWorld, leftWorld, rightWorld };
    } 

    public static bool IsOutsideBorder(Vector3 position) {
        List<Vector3> borderWorldPosition = GetScreenBorderWorldPosition(position);
        if (position.y > borderWorldPosition[0].y) return true;
        if (position.y < borderWorldPosition[1].y) return true;
        if (position.x < borderWorldPosition[2].x) return true;
        if (position.x > borderWorldPosition[3].x) return true;
        return false;
    }
    
    public static (Vector3, float) GetClosestDistanceToBorder(Vector3 position) {
        List<Vector3> borderWorldPosition = GetScreenBorderWorldPosition(position);
        Vector3 closestBorderPosition = borderWorldPosition[0];
        float closestDistance = Mathf.Infinity;

        foreach (Vector3 borderPosition in borderWorldPosition) {
            float distance = Vector3.Distance(borderPosition, position);
            if (distance >= closestDistance) continue;
            closestDistance = distance;
            closestBorderPosition = borderPosition;
        }

        return (closestBorderPosition, closestDistance);
    }
}

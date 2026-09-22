using System.Collections.Generic;
using UnityEngine;

public static class ScreenUtils {

    public static Vector3 GetMiddleWorldPosition(Transform transform) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 middleScreen = new(Screen.width / 2, Screen.height / 2, screenPos.z);
        return Camera.main.ScreenToWorldPoint(middleScreen);
    }
    
    public static Vector3 GetTopWorldPosition(Transform transform) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 topScreen = new(screenPos.x, Screen.height, screenPos.z);
        return Camera.main.ScreenToWorldPoint(topScreen);
    }

    public static Vector3 GetBottomWorldPosition(Transform transform) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 bottomScreen = new(screenPos.x, 0f, screenPos.z);
        return Camera.main.ScreenToWorldPoint(bottomScreen);
    }

    public static Vector3 GetRightWorldPosition(Transform transform) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 rightScreen = new(Screen.width, screenPos.y, screenPos.z);
        return Camera.main.ScreenToWorldPoint(rightScreen);
    }

    public static Vector3 GetLeftWorldPosition(Transform transform) {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 leftScreen = new(0f, screenPos.y, screenPos.z);
        return Camera.main.ScreenToWorldPoint(leftScreen);
    }

    public static List<Vector3> GetScreenBorderWorldPosition(Transform transform) {
        Vector3 topWorld = GetTopWorldPosition(transform);
        Vector3 bottomWorld = GetBottomWorldPosition(transform);
        Vector3 leftWorld = GetLeftWorldPosition(transform);
        Vector3 rightWorld = GetRightWorldPosition(transform);
        return new List<Vector3>() { topWorld, bottomWorld, leftWorld, rightWorld };
    } 

    public static bool IsOutsideBorder(Vector3 position, Transform transform) {
        List<Vector3> borderWorldPosition = GetScreenBorderWorldPosition(transform);
        if (position.y > borderWorldPosition[0].y) return true;
        if (position.y < borderWorldPosition[1].y) return true;
        if (position.x < borderWorldPosition[2].x) return true;
        if (position.x > borderWorldPosition[3].x) return true;
        return false;
    }
}

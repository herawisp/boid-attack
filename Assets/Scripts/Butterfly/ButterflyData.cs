using UnityEngine;

[CreateAssetMenu(fileName = "ButterflyData", menuName = "Scriptable Objects/ButterflyData")]
public class ButterflyData : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public Butterfly Prefab;
    public float Health;
    public float Cooldown;
}

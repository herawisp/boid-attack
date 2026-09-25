using UnityEngine;

[CreateAssetMenu(fileName = "ButterflyData", menuName = "Scriptable Objects/ButterflyData")]
public class ButterflyData : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public float Health, AttackDamage, Cooldown;
    public AbilityEntry[] Abilities;
}

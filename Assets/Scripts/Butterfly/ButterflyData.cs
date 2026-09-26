using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ButterflyData", menuName = "Scriptable Objects/ButterflyData")]
public class ButterflyData : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public float Health, AttackDamage, Cooldown;
    public AbilityEntry[] Abilities;
    public String DisplayName;
    public String Description;
    public Sprite Sprite;
}

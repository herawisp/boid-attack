using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { Easy, Medium, Hard }

[CreateAssetMenu(menuName = "Dungeon/Enemy Team")]
public class EnemyTeamData : ScriptableObject {
    public Difficulty Difficulty;
    public List<ButterflyType> Members;
}
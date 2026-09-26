using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Card Pack")]
public class CardPackData : ScriptableObject {
    public string PackName;
    public int Price;   // kept for later — see note below
    public Sprite Icon;
    public List<ButterflyType> PossibleButterflies;
}
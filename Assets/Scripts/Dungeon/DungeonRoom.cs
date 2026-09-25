using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonRoom : MonoBehaviour {
    
    public static DungeonRoom Instance {get; private set;}

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }


    public List<DungeonDoor> Doors;

    public void UpdateNormalRoom(List<bool> isOpen) {
        for (int i = 0; i < 4; i++) {
            Doors[i].Enable(isOpen[i]);
        }
    }
}

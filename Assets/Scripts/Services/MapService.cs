using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapService : MonoBehaviour {

    public static MapService Instance {get; private set;}

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    //================================================================================================//
    //================================================================================================//

    public Image DungeonRoomImage;
    public List<Sprite> RoomSprites;

    private Transform _roomCells;

    //================================================================================================//
    //================================================================================================//

    void Start() {
        _roomCells = transform.Find("RoomCells");
    }

    public void UpdateMap() {
        DungeonData dungeonData = FloorService.Instance.DungeonData;
        GridLayoutGroup gridLayoutGroup = _roomCells.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = dungeonData.Width;

        for (int i = _roomCells.childCount - 1; i >= 0; i--) {
            Destroy(_roomCells.GetChild(i).gameObject);
        }

        for (int i = 0; i < dungeonData.Cells.Count(); i++) {
            Sprite sprite;
            if (i == dungeonData.CurrentCell) {
                sprite = RoomSprites[6];
            } else sprite = RoomSprites[(int) dungeonData.Cells[i]];

            Image dungeonRoomImage = Instantiate(DungeonRoomImage, _roomCells);
            dungeonRoomImage.sprite = sprite;
            dungeonRoomImage.name = i.ToString();

            if (sprite == null) dungeonRoomImage.color = new(1,1,1,0);
        }
    }

    void OnOpenMap() {
        _roomCells.gameObject.SetActive(!_roomCells.gameObject.activeSelf);
    }
    
    //================================================================================================//
    //================================================================================================//
}

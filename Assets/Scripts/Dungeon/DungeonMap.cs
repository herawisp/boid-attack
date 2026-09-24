using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMap : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    public Image DungeonRoomImage;
    public List<Sprite> RoomSprites;

    private Transform _roomCells;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _roomCells = transform.Find("RoomCells");
    }

    void Start() {
        DungeonGenerator dungeonGenerator = new();
        DungeonData dungeonData = dungeonGenerator.Generate();

        dungeonData.Trim();
        GridLayoutGroup gridLayoutGroup = _roomCells.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = dungeonData.Width;

        for (int i = 0; i < dungeonData.Cells.Count(); i++) {
            Sprite sprite = RoomSprites[(int) dungeonData.Cells[i]];
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

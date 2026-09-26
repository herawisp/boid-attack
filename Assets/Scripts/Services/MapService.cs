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
            } else if (dungeonData.Visited[i]) {
                sprite = RoomSprites[(int) dungeonData.Cells[i]];
            } else if (IsAdjacentToVisited(dungeonData, i)) {
                sprite = null;//RoomSprites[7];
            } else {
                sprite = null;
            }

            Image dungeonRoomImage = Instantiate(DungeonRoomImage, _roomCells);
            dungeonRoomImage.sprite = sprite;
            dungeonRoomImage.name = i.ToString();

            if (sprite == null) dungeonRoomImage.color = new(1,1,1,0);
        }
    }

    bool IsAdjacentToVisited(DungeonData d, int i) {
        int width = d.Width, x = i % width;
        return (i >= width && d.Visited[i - width]) ||
            (i < d.Cells.Length - width && d.Visited[i + width]) ||
            (x > 0 && d.Visited[i - 1]) ||
            (x < width - 1 && d.Visited[i + 1]);
    }

    void OnOpenMap() {
        _roomCells.gameObject.SetActive(!_roomCells.gameObject.activeSelf);
    }
    
    //================================================================================================//
    //================================================================================================//
}

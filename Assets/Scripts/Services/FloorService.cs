using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class FloorService : MonoBehaviour {

    public static FloorService Instance {get; private set;}

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    
    //================================================================================================//
    //================================================================================================//

    public DungeonData DungeonData;
    public RectTransform Dungeon;
    public RectTransform Transition;

    //================================================================================================//
    //================================================================================================//

    public void GenerateFloor() {
        DungeonGenerator dungeonGenerator = new();
        DungeonData = dungeonGenerator.Generate();
        MapService.Instance.UpdateMap();
        UpdateDungeonRoom();
    }
    
    public void MoveUp()
    {
        TransitionUp(() => {
            TryMove(DungeonData.CurrentCell - DungeonData.Width);  
        });
    }

    public void MoveDown()  => TryMove(DungeonData.CurrentCell + DungeonData.Width);
    public void MoveLeft()  => TryMove(DungeonData.CurrentCell - 1);
    public void MoveRight() => TryMove(DungeonData.CurrentCell + 1);

    void TryMove(int targetCell) {
        List<bool> neighbours = GetNeighboursRoom(DungeonData.CurrentCell);
        int direction = targetCell - DungeonData.CurrentCell;
        int width = DungeonData.Width;

        bool canMove =
            (direction == -width && neighbours[0]) || // top
            (direction == width  && neighbours[1]) || // bottom
            (direction == -1     && neighbours[2]) || // left
            (direction == 1      && neighbours[3]);   // right

        if (!canMove) return;

        DungeonData.CurrentCell = targetCell;
        MapService.Instance.UpdateMap();
        UpdateDungeonRoom();
    }

    List<bool> GetNeighboursRoom(int i) {
        int width = DungeonData.Width;
        int x = i % width;
        var cells = DungeonData.Cells;

        return new List<bool> {
            i >= width                && cells[i - width] != RoomType.None,  // top
            i < cells.Length - width  && cells[i + width] != RoomType.None,  // bottom
            x > 0                     && cells[i - 1]     != RoomType.None,  // left
            x < width - 1             && cells[i + 1]     != RoomType.None   // right
        };
    }

    void UpdateDungeonRoom() {
        RoomType roomType = DungeonData.Cells[DungeonData.CurrentCell];
        DungeonRoom.Instance.UpdateNormalRoom(GetNeighboursRoom(DungeonData.CurrentCell));
        // if (roomType == RoomType.Empty || roomType == RoomType.Battle) {

        //     DungeonRoom.Instance.UpdateNormalRoom(GetNeighboursRoom(DungeonData.CurrentCell));
        // }
    }
    
    //================================================================================================//
    //================================================================================================//
    
    void TransitionUp(TweenCallback onComplete = null) {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(Transition.DOAnchorPosY(600, 1f));
        sequence.AppendCallback(() => Dungeon.anchoredPosition = new Vector2(0, 0));
        sequence.Append(Transition.DOAnchorPosY(-1140, 1f));

        if (onComplete != null) sequence.OnComplete(onComplete);
    }


    //================================================================================================//
    //================================================================================================//
}

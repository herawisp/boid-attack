using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator {
    
    //================================================================================================//
    //================================================================================================//

    [Header("Dungeon Configuration")]
    [SerializeField] private int _maxRooms = 15;
    [SerializeField] private int _minRooms = 7;

    private DungeonData _data;
    private int _roomCount;
    private List<int> _cellQueue;
    private List<int> _endRooms;

    //================================================================================================//
    //================================================================================================//

    public DungeonData Generate() {
        DungeonData result;
        while ((result = TryGenerate()) == null ) { }
        return result;
    }

    //================================================================================================//
    //================================================================================================//

    private DungeonData TryGenerate() {
        _data = new DungeonData();
        _roomCount = 0;
        _cellQueue = new List<int>();
        _endRooms = new List<int>();

        Visit(45);

        while (_cellQueue.Count > 0) {
            int i = _cellQueue[0];
            _cellQueue.RemoveAt(0);
            int x = i % 10;

            bool created = false;
            if (x > 1) created |= Visit(i - 1);
            if (x < 9) created |= Visit(i + 1);
            if (i > 20) created |= Visit(i - 10);
            if (i < 70) created |= Visit(i + 10);
            if (!created) _endRooms.Add(i);
        }

        if (_roomCount < _minRooms || _endRooms.Count < 3) return null;

        _data.Cells[PopRandomEndRoom()] = RoomType.Shop;
        _data.Cells[PopRandomEndRoom()] = RoomType.Key;
        _data.Cells[PopRandomEndRoom()] = RoomType.Exit;

        return _data;
    }

    private int PopRandomEndRoom() {
        int index = Random.Range(0, _endRooms.Count);
        int i = _endRooms[index];
        _endRooms.RemoveAt(index);
        return i;
    }

    private int NeighbourCount(int i) {
        int up = (i + 10 < 100) && _data.Cells[i + 10] != RoomType.None ? 1 : 0;
        int down = (i - 10 >= 0) && _data.Cells[i - 10] != RoomType.None ? 1 : 0;
        int right = (i - 1 >= 0) && _data.Cells[i - 1] != RoomType.None ? 1 : 0;
        int left = (i + 1 < 100) && _data.Cells[i + 1] != RoomType.None ? 1 : 0;
        return up + down + right + left;
    }

    private bool Visit(int i) {
        if (_data.Cells[i] != RoomType.None) return false;
        if (NeighbourCount(i) > 1) return false;
        if (_roomCount >= _maxRooms) return false;
        if (Random.Range(0f, 1f) < 0.5f && i != 45) return false;

        _cellQueue.Add(i);
        _data.Cells[i] = RoomType.Empty;
        _roomCount++;
        return true;
    }

    //================================================================================================//
    //================================================================================================//

}

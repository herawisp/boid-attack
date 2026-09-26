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

        for (int i = 0; i < _data.Cells.Length; i++) {
            if (_data.Cells[i] == RoomType.Empty && i != 45)
                _data.Cells[i] = RoomType.Battle;
        }

        Trim();
        _data.Visited = new bool[_data.Cells.Length];
        _data.Cleared = new bool[_data.Cells.Length];
        return _data;
    }

    public void Trim() {
        int minCol = 10, maxCol = -1;
        int minRow = 10, maxRow = -1;

        for (int i = 0; i < _data.Cells.Length; i++) {
            if (_data.Cells[i] == RoomType.None) continue;
            int col = i % 10;
            int row = i / 10;
            minCol = Mathf.Min(minCol, col);
            maxCol = Mathf.Max(maxCol, col);
            minRow = Mathf.Min(minRow, row);
            maxRow = Mathf.Max(maxRow, row);
        }

        if (maxCol < 0) return;

        int newWidth = maxCol - minCol + 1;
        int newHeight = maxRow - minRow + 1;
        RoomType[] trimmed = new RoomType[newWidth * newHeight];

        for (int row = minRow; row <= maxRow; row++) {
            for (int col = minCol; col <= maxCol; col++) {
                trimmed[(row - minRow) * newWidth + (col - minCol)] = _data.Cells[row * 10 + col];
            }
        }

        int startCol = 45 % 10 - minCol;
        int startRow = 45 / 10 - minRow;
        _data.StartCell = startRow * newWidth + startCol;
        _data.CurrentCell = _data.StartCell;

        _data.Cells = trimmed;
        _data.Width = newWidth;
        _data.Height = newHeight;
        _data.Visited = new bool[trimmed.Length];
        _data.Cleared = new bool[trimmed.Length];
    }

    private int PopRandomEndRoom() {
        int index = Random.Range(0, _endRooms.Count);
        int i = _endRooms[index];
        _endRooms.RemoveAt(index);
        return i;
    }

    private int NeighbourCount(int i) {
        int x = i % 10;
        int up = (i + 10 < 100) && _data.Cells[i + 10] != RoomType.None ? 1 : 0;
        int down = (i - 10 >= 0) && _data.Cells[i - 10] != RoomType.None ? 1 : 0;
        int right = (x < 9) && _data.Cells[i + 1] != RoomType.None ? 1 : 0;
        int left = (x > 0) && _data.Cells[i - 1] != RoomType.None ? 1 : 0;
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

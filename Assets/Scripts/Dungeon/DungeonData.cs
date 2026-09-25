
using System;

public enum RoomType {None = 0, Empty = 1, Battle = 2, Shop = 3, Key = 4, Exit = 5}

public class DungeonData {

    public RoomType[] Cells = new RoomType[100];
    public int Width = 10;
    public int Height = 10;
    public int StartCell = 45;
    public int CurrentCell = 45;
}

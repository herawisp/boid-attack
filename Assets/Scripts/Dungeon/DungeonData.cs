
using System;

public enum RoomType {None = 0, Empty = 1, Battle = 2, Shop = 3, Key = 4, Exit = 5}

public class DungeonData {
    
    public const int GridSize = 10;

    public RoomType[] Cells = new RoomType[100];
    public int Width = GridSize;
    public int Height = GridSize;
    public int StartCell = 45;

    //================================================================================================//
    //================================================================================================//

    public void Trim() {
        int minCol = Width, maxCol = -1;
        int minRow = Height, maxRow = -1;

        for (int i = 0; i < Cells.Length; i++) {
            if (Cells[i] == RoomType.None) continue;
            int col = i % Width;
            int row = i / Width;
            minCol = Math.Min(minCol, col);
            maxCol = Math.Max(maxCol, col);
            minRow = Math.Min(minRow, row);
            maxRow = Math.Max(maxRow, row);
        }

        if (maxCol < 0) return;

        int newWidth = maxCol - minCol + 1;
        int newHeight = maxRow - minRow + 1;
        RoomType[] trimmed = new RoomType[newWidth * newHeight];

        for (int row = minRow; row <= maxRow; row++) {
            for (int col = minCol; col <= maxCol; col++) {
                trimmed[(row - minRow) * newWidth + (col - minCol)] = Cells[row * Width + col];
            }
        }

        int startCol = StartCell % Width - minCol;
        int startRow = StartCell / Width - minRow;
        StartCell = startRow * newWidth + startCol;

        Cells = trimmed;
        Width = newWidth;
        Height = newHeight;
    }
    
    //================================================================================================//
    //================================================================================================//
}

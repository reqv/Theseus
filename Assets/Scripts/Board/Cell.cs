using System;

/// <summary>
/// Enumerator odpowiadający za rodzaje pokoi (komórek na planszy).
/// </summary>
public enum CellType
{
    Empty = 0,
    Common = 1,
    Start = 2,
    Boss = 3,
    Premium = 4,
    Shop = 5

}

public class Cell
{
    public CellType Type { get; set; }
    public CellType TopNeighbour { get; set; }
    public CellType RightNeighbour { get; set; }
    public CellType BottomNeighbour { get; set; }
    public CellType LeftNeighbour { get; set; }

    public Cell()
    {
        Type = CellType.Empty;
    }

    public Cell(CellType type)
    {
        Type = type;
    }
}

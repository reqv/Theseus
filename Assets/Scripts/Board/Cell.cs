using System;

/// <summary>
/// Model reprezentujący typ pokoju oraz jego sąsiadów
/// </summary>
public class Cell
{
    /// <summary>
    /// Typ danej komórki
    /// </summary>
    public CellType Type { get; set; }
    /// <summary>
    /// Typ komórki górnego sąsiada
    /// </summary>
    public CellType TopNeighbour { get; set; }
    /// <summary>
    /// Typ komórki prawgo sąsiada
    /// </summary>
    public CellType RightNeighbour { get; set; }
    /// <summary>
    /// Typ komórki dolnego sąsiada
    /// </summary>
    public CellType BottomNeighbour { get; set; }
    /// <summary>
    /// Typ komórki lewego sąsiada
    /// </summary>
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

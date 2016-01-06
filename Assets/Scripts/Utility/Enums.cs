/// <summary>
/// Enum z dozwolonymi kierunkami w labiryncie
/// </summary>
public enum Direction
{
    Top = 0,
    Right,
    Bottom,
    Left
}


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

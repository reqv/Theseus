using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public RoomManager roomManager;

    private const int _boardSize = 9;
    private Board _board = new Board(_boardSize);
    private int _actualPositionX;
    private int _actualPositionY;

    private int _level = 3;

    // Use this for initialization
    void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        roomManager = GetComponent<RoomManager>();
        InitGame();
    }

    void InitGame()
    {
        _board.FillBoard();
        _actualPositionX = _actualPositionY = _board.HalfOfBoardSize;

        roomManager.SetupRoom(_level, _board[_actualPositionX, _actualPositionY]);
    }

}

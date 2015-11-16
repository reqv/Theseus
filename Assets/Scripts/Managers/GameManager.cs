using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public RoomManager roomManager;

    private const int _boardSize = 9;
    private Board _board = new Board(_boardSize);
    private int _actualPositionX;
    private int _actualPositionY;

    private int _level = 1;
    private Dictionary<Vector2, GameObject> _roomGameObjectsHolder = new Dictionary<Vector2,GameObject>();
    private GameObject _actualRoom;

    // Use this for initialization
    void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        //roomManager = GetComponent<RoomManager>();
        AddListeners();
        InitGame();
    }

    void InitGame()
    {
        _board.FillBoard();
        _actualPositionX = _actualPositionY = _board.HalfOfBoardSize;

        GoToRoom(_actualPositionX, _actualPositionY);
    }

    void AddListeners()
    {
        Messenger.AddListener<Direction>(Messages.PlayerGoesThroughTheDoor, OnRoomChange);
    }

    void OnRoomChange(Direction direction)
    {
        switch(direction)
        {
            case Direction.Top:
                _actualPositionY++;
                break;
            case Direction.Bottom:
                _actualPositionY--;
                break;
            case Direction.Right:
                _actualPositionX++;
                break;
            case Direction.Left:
                _actualPositionX--;
                break;
        }

        GoToRoom(_actualPositionX, _actualPositionY);
    }

    void GoToRoom(int x, int y)
    {
        GameObject destinationRoom;
        var vector = new Vector2(x, y);

        if (_roomGameObjectsHolder.ContainsKey(vector))
            destinationRoom = _roomGameObjectsHolder[vector];
        else
        {
            destinationRoom = roomManager.SetupRoom(_level, _board[_actualPositionX, _actualPositionY]);
            _roomGameObjectsHolder.Add(vector, destinationRoom);
        }

        if(_actualRoom != null)
            _actualRoom.SetActive(false);
        
        _actualRoom = destinationRoom;
        _actualRoom.SetActive(true);

        FindObjectsOfType<Door>().ToList().ForEach(door => door.Open());
    }

    void SpawnEnemies()
    {

    }
}

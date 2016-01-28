using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/**
 * <summary>
 * 	Główna klasa zarządzająca stanem gry
 * </summary>
 * <remarks>
 *  W czasie uruchamiania sceny z grą tworzy labirynt i umieszcza w nim gracza
 * </remarks>
 */
public class GameManager : MonoBehaviour
{
    public static GameObject Theseus;
    public static Canvas GUI;

    [SerializeField]
    /// <summary>
    /// Manager pomieszczeń dla poziomów 1-5
    /// </summary>
    private RoomManager _sandRoomManager;
    [SerializeField]
    /// <summary>
    /// Manager pomieszczeń dla poziomów 6-10
    /// </summary>
    private RoomManager _stoneRoomManager;
    [SerializeField]
    /// <summary>
    /// Manager pomieszczeń dla poziomów 11-15
    /// </summary>
    private RoomManager _finalRoomManager;

    [HideInInspector]
    public RoomManager roomManager;

    /// <summary>
    /// Rozmiar planszy na której umieszcane mogą być komóki labiryntu
    /// </summary>
    private const int _boardSize = 9;
    /// <summary>
    /// Plansza reprezentująca labirynt
    /// </summary>
    private Board _board = new Board(_boardSize);
    /// <summary>
    /// Współrzędna X w labiryncie aktualnego pokoju
    /// </summary>
    private int _actualPositionX;
    /// <summary>
    /// Współrzędna Y w labiryncie aktualnego pokoju
    /// </summary>
    private int _actualPositionY;

    /// <summary>
    /// Numer poziomu
    /// </summary>
    public static int _level = 1;
    /// <summary>
    /// Kolekcja przetrzymująca wszystkie pokoje wraz z ich współrzędnymi w grze
    /// </summary>
    private Dictionary<Vector2, GameObject> _roomGameObjectsHolder = new Dictionary<Vector2,GameObject>();
    /// <summary>
    /// Odwołanie do pokoju w którym znajduje się gracz
    /// </summary>
    private GameObject _actualRoom;

    public GameObject ActualRoom { get { return _actualRoom; } }

    // Use this for initialization
    void Start ()
    {
        //if (instance == null)
        //    instance = this;
        //else if (instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

        if (_level == 1)
        {
            var toDelete = FindObjectOfType<MainCharacter>();
            if (toDelete != null)
                Destroy(toDelete.gameObject);
            var toDelete2 = FindObjectOfType<GUIManager>();
            if (toDelete2 != null)
                Destroy(toDelete2.gameObject);
            Application.LoadLevelAdditive("test");
        }

        if (_level >= 5)
            roomManager = _finalRoomManager;
        else if (_level >= 3)
            roomManager = _stoneRoomManager;
        else
            roomManager = _sandRoomManager;

        AddListeners();
        InitGame();
    }

    /// <summary>
    /// Metoda inicjująca grę - losuje i tworzy labirynt
    /// </summary>
    void InitGame()
    {
        _board.FillBoard();
        _actualPositionX = _actualPositionY = _board.HalfOfBoardSize;

        GoToRoom(_actualPositionX, _actualPositionY);
    }

    /// <summary>
    /// Metoda w której dodawani są "nasłuchiwacze" zdarzeń
    /// </summary>
    void AddListeners()
    {
        Messenger.AddListener<Direction>(Messages.PlayerGoesThroughTheDoor, OnRoomChange);
    }

    /// <summary>
    /// Metoda reagująca na przejście gracza przez drzwi, przenosi go do następnego pokoju
    /// </summary>
    /// <param name="direction"></param>
    void OnRoomChange(Direction direction)
    {
        Debug.Log("Hejka");
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

    /// <summary>
    /// Przenosi gracza do pokoju o zadanych współrzędnych w labiryncie
    /// </summary>
    /// <param name="x">Współrzędna x w labiryncie</param>
    /// <param name="y">Współrzędna y w labiryncie</param>
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
    }

    /// <summary>
    /// Metoda wywoływana przy pokonaniu boss'a i zejściu na kolejny poziom labiryntu
    /// </summary>
    public void NextLevel()
    {
        _level++;
        if (_level == 3)
            roomManager = _stoneRoomManager;
        if (_level == 5)
            roomManager = _finalRoomManager;
        if (_level == 6)
        {
            EndGame();
            return;
        }

        Application.LoadLevel(Application.loadedLevel);
    }

    /// <summary>
    /// Metoda wywoływana po skończeniu gry
    /// </summary>
    public void EndGame()
    {

    }
}

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    #region SerializedFields
    [Tooltip("Szerokość pokoju")]
    [SerializeField]
    /// <summary>
    /// Szerokośc pokoju
    /// </summary>
    private int _columns;
    [Tooltip("Wysokość pokoju")]
    [SerializeField]
    /// <summary>
    /// Wysokośc pokoju
    /// </summary>
    private int _rows;
    [SerializeField]
    private Count _foodCount = new Count(1, 5);
    [Tooltip("Prefab drzwi")]
    [SerializeField]
    /// <summary>
    /// Prefab drzwi
    /// </summary>
    private GameObject _exit;
    [Tooltip("Prefaby podłogi")]
    [SerializeField]
    /// <summary>
    /// Prefaby podłogi
    /// </summary>
    private GameObject[] _floorTiles;
    [Tooltip("Prefaby ścian zewnętrznych")]
    [SerializeField]
    /// <summary>
    /// Prefaby ścian zewnętrznych
    /// </summary>
    private GameObject[] _outerWallTiles;
    [Tooltip("Prefaby narożników ścian zewnętrznych")]
    [SerializeField]
    /// <summary>
    /// Prefaby narożników ścian zewnętrznych
    /// </summary>
    private GameObject[] _outerWallCornerTiles;


    [SerializeField]
    /// <summary>
    /// Używany menedżer potworów
    /// </summary>
    private MonstersManager _monstersManager;
    [SerializeField]
    /// <summary>
    /// Prefab sklepikarza
    /// </summary>
    private ShopKeeper _shopKeeper;
    #endregion

    /// <summary>
    /// GameObject reprezentujący pokój
    /// </summary>
    private Transform _roomHolder;
    /// <summary>
    /// Kolekcja współrzędnych wszystkich elementów pokoju
    /// </summary>
    private List<Vector3> _gridPositions = new List<Vector3>();
    /// <summary>
    /// Model "komórki" labiryntu
    /// </summary>
    private Cell _roomCell;

    /// <summary>
    /// Zwraca wysokość pokoju
    /// </summary>
    public int Rows { get { return _rows; } }
    /// <summary>
    /// Zwraca szerokość pokoju
    /// </summary>
    public int Columns { get { return _columns; } }

    /// <summary>
    /// Metoda uzupełniająca pole _gridPositions 
    /// </summary>
    void InitialiseList ()
    {
        _gridPositions.Clear();

        for (int x = 0; x < _columns - 1; x++)
        {
            for (int y = 0; y < _rows - 1; y++)
            {
                _gridPositions.Add(new Vector3(x,y,0f));
            }
        }
    }

    /// <summary>
    /// Metoda losująca i tworząca ściany i podłogę pokoju
    /// </summary>
    void RoomSetup ()
    {
        _roomHolder = new GameObject("Room").transform;
        float rotation = 0.0f;
        int outerWall = 0;

        for (int x = -1; x < _columns + 1; x++)
        {
            for (int y = -1; y < _rows + 1; y++)
            {
                GameObject toInstantiate = _floorTiles[Random.Range(0, _floorTiles.Length)];
                rotation = 0;
                outerWall = 0;
                
                if (x == -1)
                {
                    rotation = 90;
                    outerWall++;
                }
                else if(x == _columns)
                {
                    rotation = 270;
                    outerWall++;
                } 
                if (y == -1)
                {
                    if (x != -1)
                        rotation = 180;
                    outerWall++;
                }
                else if (y == _rows)
                {
                    if(x != _columns)
                    rotation = 0;
                    outerWall++;
                }

                if(outerWall == 1)
                    toInstantiate = _outerWallTiles[Random.Range(0, _outerWallTiles.Length)];
                else if(outerWall == 2)
                    toInstantiate = _outerWallCornerTiles[Random.Range(0, _outerWallCornerTiles.Length)];


                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.Rotate(0, 0, rotation);

                instance.transform.SetParent(_roomHolder);
            }
        }
    }

    /// <summary>
    /// Metoda umieszczająca drzwi
    /// </summary>
    void PlaceDoors()
    {
        if (_roomCell.TopNeighbour != CellType.Empty)
        {
            var instance = Instantiate(_exit, new Vector3(_columns / 2, _rows, 0f), Quaternion.identity) as GameObject;
            instance.GetComponent<Door>().Direction = Direction.Top;
            instance.transform.SetParent(_roomHolder);

            var wallCollider = _roomHolder.GetComponentsInChildren<Transform>().First(t => t.position.x == instance.transform.position.x &&
                t.position.y == instance.transform.position.y).gameObject.GetComponent<Collider2D>();

            instance.GetComponent<Door>().ClosedDoorCollider = wallCollider;
        }

        if (_roomCell.RightNeighbour != CellType.Empty)
        {
            var instance = Instantiate(_exit, new Vector3(_columns, _rows / 2, 0f), Quaternion.identity) as GameObject;
            instance.transform.Rotate(0, 0, -90);
            instance.GetComponent<Door>().Direction = Direction.Right;
            instance.transform.SetParent(_roomHolder);

            var wallCollider = _roomHolder.GetComponentsInChildren<Transform>().First(t => t.position.x == instance.transform.position.x &&
                t.position.y == instance.transform.position.y).gameObject.GetComponent<Collider2D>();

            instance.GetComponent<Door>().ClosedDoorCollider = wallCollider;
        }

        if (_roomCell.BottomNeighbour != CellType.Empty)
        {
            var instance = Instantiate(_exit, new Vector3(_columns / 2, -1, 0f), Quaternion.identity) as GameObject;
            instance.transform.Rotate(0, 0, 180);
            instance.GetComponent<Door>().Direction = Direction.Bottom;
            instance.transform.SetParent(_roomHolder);

            var wallCollider = _roomHolder.GetComponentsInChildren<Transform>().First(t => t.position.x == instance.transform.position.x &&
                t.position.y == instance.transform.position.y).gameObject.GetComponent<Collider2D>();

            instance.GetComponent<Door>().ClosedDoorCollider = wallCollider;
        }

        if (_roomCell.LeftNeighbour != CellType.Empty)
        {
            var instance = Instantiate(_exit, new Vector3(-1, _rows / 2, 0f), Quaternion.identity) as GameObject;
            instance.transform.Rotate(0, 0, 90);
            instance.GetComponent<Door>().Direction = Direction.Left;
            instance.transform.SetParent(_roomHolder);

            var wallCollider = _roomHolder.GetComponentsInChildren<Transform>().First(t => t.position.x == instance.transform.position.x &&
                t.position.y == instance.transform.position.y).gameObject.GetComponent<Collider2D>();

            instance.GetComponent<Door>().ClosedDoorCollider = wallCollider;
        }
    }

    /// <summary>
    /// Metoda do losowania pozycji w pokoju
    /// </summary>
    /// <returns>Współrzędne losowej pozycji</returns>
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    /// <summary>
    /// Umieszcza obiekty w pokoju w losowych miejscach
    /// </summary>
    /// <param name="tileArray">Tablica elementów do rozmieszczenia</param>
    /// <param name="minimum">Minimalna ilość elementów</param>
    /// <param name="maximum">Maksymalna ilość elementów</param>
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Metoda tworząca sklep w danym pokoju
    /// </summary>
    /// <param name="level">Nr poziomu</param>
    void SetupShop(int level)
    {
        Debug.Log("Witamy w sklepie!");
        Vector3 shopKeeperPosition;

        if(_roomCell.TopNeighbour != CellType.Empty)
        {
            shopKeeperPosition = new Vector3(2, -0.4f, 0);
        }
        else
        {
            shopKeeperPosition = new Vector3(2, 2.25f, 0);
        }

        var sk = Instantiate(_shopKeeper, shopKeeperPosition, Quaternion.identity) as ShopKeeper;
        sk.transform.SetParent(_roomHolder.transform);
    }

    /// <summary>
    /// Metoda tworząca cały pokój
    /// </summary>
    /// <param name="level">Nr poziomu</param>
    /// <param name="cell">Komórka labiryntu która reprezentuje tworzony pokój</param>
    /// <returns></returns>
    public GameObject SetupRoom(int level, Cell cell)
    {
        _roomCell = cell;

        RoomSetup();
        InitialiseList();
        PlaceDoors();

        if(cell.Type == CellType.Common)
        {
            _monstersManager.SpawnEnemies(level, _roomHolder.gameObject);
            _roomHolder.gameObject.GetComponentsInChildren<Door>().ToList().ForEach(door => door.Close());
        }
        else
        {
            _roomHolder.gameObject.GetComponentsInChildren<Door>().ToList().ForEach(door => door.Open());
        }

        if(cell.Type == CellType.Shop)
        {
            SetupShop(level);
        }

        return _roomHolder.gameObject;
    }
}

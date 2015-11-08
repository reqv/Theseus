using System;
using System.Collections.Generic;
using System.Linq;

/**
 * <summary>
 * 	Klasa generująca proceduralnie planszę danego poziomu. 
 * </summary>
 * <remarks>
 * 	Klasa odpowiadająca za wygenerowanie rozmieszczenia pokoi.
 * </remarks>
 */
public class Board
{
    /**
     * <summary>
     * 	Klasa pomocnicza do określania współrzędnych. 
     * </summary>
     * <remarks>
     * 	Klasa pomocna przy kopiowaniu współrzędnych tabeli w jednej z metod.
     * </remarks>
     */
    private class Coords
    {

        /// <summary>
        /// Współrzędna X - Kolumna
        /// </summary>
        public int X
        {
            get; set;
        }
        /// <summary>
        /// Współrzędna Y - Wiersz
        /// </summary>
        public int Y
        {
            get; set;
        }

        /// <summary>
        /// Konstruktor klasy Coords przyjmujący dwie wartości.
        /// </summary>
        /// <param name="x">Współrzędna X - Kolumna</param>
        /// <param name="y">Współrzędna Y - Wiersz</param>
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
<<<<<<< HEAD:Assets/Scripts/Classes/Board.cs
    /// Enumerator odpowiadający za rodzaje pokoi (komórek w tabeli).
    /// </summary>
    private enum Cell
    {
        /// <summary>
        /// Brak pokoju
        /// </summary>
        Empty = 0,
        /// <summary>
        /// Zwykły pokój
        /// </summary>
        Common = 1,
        /// <summary>
        /// Pokój startowy
        /// </summary>
        Start = 2,
        /// <summary>
        /// Pokój ze Strażnikiem
        /// </summary>
        Boss = 3,
        /// <summary>
        /// Pokój z unikalnym przedmiotem
        /// </summary>
        Premium = 4,
        /// <summary>
        /// Pokój ze sklepem
        /// </summary>
        Shop = 5

    }

    /// <summary>
=======
>>>>>>> c988aed30ab75eb3052d2af5d49c8fc66b18cec8:Assets/Scripts/Board/Board.cs
    /// Obiekt klasy Random odpowiadający za losowanie.
    /// </summary>
    private Random _rand;

    private int _minRoomCount = 15;
    /// <summary>
    /// Minimalna ilość pokoi (domyślnie 15).
    /// </summary>
    public int MinRoomCount
    {
        get
        {
            return _minRoomCount;
        }
        set
        {
            _minRoomCount = value;
        }
    }

    private int _maxRoomCount = 20;
    /// <summary>
    /// Maksymalna ilość pokoi (domyślnie 20).
    /// </summary>
    public int MaxRoomCount
    {
        get
        {
            return _maxRoomCount;
        }
        set
        {
            _maxRoomCount = value;
        }
    }

    /// <summary>
    /// Właściwość klasy Board określająca rozmiar planszy.
    /// </summary>
    /// <value>
    /// Rozmiar planszy.
    /// </value>
    public int BoardSize
    {
        get
        {
            return BoardSize;
        }
        set
        {
            HalfOfBoardSize = value / 2;
            BoardSize = value;
        }
    }

    /// <summary>
    /// Tablica zawierająca informację o rozmieszczeniu pokoi.
    /// </summary>
    public Cell[,] BoardTab
    {
        get;
        set;
    }

    /// <summary>
    /// Połowa rozmiaru planszy, ustawiana w konstruktorze klasy <see cref="Board"/>.
    /// </summary>
    public int HalfOfBoardSize
    {
        get;
        private set;
    }

    public Cell this[int x, int y]
    {
        get { return BoardTab[x, y]; }
    }

    /// <summary>
    /// Konstruktor klasy Board określający rozmiar planszy.
    /// </summary>
    /// <param name="boardSize">Rozmiar planszy.</param>
    public Board(int boardSize)
    {
        BoardTab = new Cell[boardSize, boardSize];
        BoardSize = boardSize;
        ResetBoard();
    }

    /// <summary>
    /// Metoda odpowiedzialna za wyczyszczenie planszy (wypełnia komórki wartością 0).
    /// </summary>
    /// <param name="boardSize">Rozmiar planszy.</param>
    /// <param name="board">Czyszczona plansza.</param>
    public void ResetBoard()
    {
        for (int i = 0; i < BoardSize; i++)
            for (int j = 0; j < BoardSize; j++)
                BoardTab[i, j] = new Cell(CellType.Empty);
    }

    /// <summary>
    /// Główna metoda odpowiedzialna za wygenerowanie rozmieszczenia pokoi.
    /// </summary>
    /// <param name="seed">Ziarno losowości (domyślnie pusty). Jeśli puste generuje nowe ziarno.</param>
    /// <returns>Ziarno rozmieszczenia pokoi.</returns>
    public string FillBoard(string seed = "")
    {
        const int roomOffset = 3;
        const int roomCoreCount = 3;
        const int seedLength = 7;

        if (string.IsNullOrEmpty(seed))
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomr = new Random();
            seed = new string(Enumerable.Repeat(chars, seedLength).Select(s => s[randomr.Next(s.Length)]).ToArray());
        }

        _rand = new Random(seed.GetHashCode());

        BoardTab[HalfOfBoardSize, HalfOfBoardSize] = new Cell(CellType.Start);
        RandomNeighbors(1);
        MakeCoreRoom(roomOffset, roomCoreCount);

        return seed;
    }

    /// <summary>
    /// Metoda pomocnicza (<see cref="FillBoard(string)"/>) odpowiadająca za ilość sąsiadów danej komórki.
    /// </summary>
    /// <param name="roomCounter">Liczba pokoi znajdujących się już w tabeli.</param>
    private void RandomNeighbors(int roomCounter)
    {
        bool isRandomized = false;

        int columnLowerLimit = HalfOfBoardSize - 1;
        int rowLowerLimit = HalfOfBoardSize - 1;
        int columnUpperLimit  = HalfOfBoardSize - 1;
        int rowUpperLimit  = HalfOfBoardSize - 1;

        do
        {
            for (int column = columnLowerLimit; column < BoardSize - columnUpperLimit; column++)
            {
                for (int row = rowLowerLimit; row < BoardSize - rowUpperLimit; row++)
                {
                    if (MakeRoom(column, row))
                    {
                        roomCounter++;
                        if (column == columnLowerLimit)
                            columnLowerLimit = columnLowerLimit == 0 ? 0 : columnLowerLimit - 1;
                        else if (column == columnUpperLimit)
                            columnUpperLimit = columnUpperLimit == 0 ? 0 : columnUpperLimit - 1;
                        if (row == rowLowerLimit)
                            rowLowerLimit = rowLowerLimit == 0 ? 0 : rowLowerLimit - 1;
                        else if (row == rowUpperLimit)
                            rowUpperLimit = rowUpperLimit == 0 ? 0 : rowUpperLimit - 1;
                    }
                    if (roomCounter >= MinRoomCount)
                    {
                        isRandomized = true;
                    }
                    if (roomCounter >= MaxRoomCount)
                        break;
                }
                if (roomCounter >= MaxRoomCount)
                    break;
            }
        } while (!isRandomized);
    }

    /// <summary>
    /// Metoda pomocnicza (<see cref="MakeRoom(int, int)"/>) zliczająca sąsiadów danej komórki tabeli.
    /// </summary>
    /// <param name="x">Indeks kolumny tabeli.</param>
    /// <param name="y">Indeks wiersza tabeli.</param>
    /// <param name="hasCloseNeighbours">Parametr typu out zwracający nam informację o tym czy dana komórka ma sąsiada w kierunku N/E/S/W.</param>
    /// <param name="checkOnlyCloseNeighbours">Parametr domyślny (false) decydujący o tym czy mają być sprawdzani wszyscy sąsiedzi wokół komórki czy tylko w podstawowych kierunkach N/E/S/W.</param>
    /// <returns>Ilość sąsiadów</returns>
    private int NumberOfNeighbors(int x, int y, out bool hasCloseNeighbours, bool checkOnlyCloseNeighbours = false)
    {
        hasCloseNeighbours = false;
        int numOfNeighbors = 0;

        if (y - 1 >= 0)
            if (BoardTab[x, y - 1].Type != CellType.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (x + 1 < BoardSize)
            if (BoardTab[x + 1, y].Type != CellType.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (y + 1 < BoardSize)
            if (BoardTab[x, y + 1].Type != CellType.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (x - 1 >= 0)
            if (BoardTab[x - 1, y].Type != CellType.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (!checkOnlyCloseNeighbours)
        {
            if (x - 1 >= 0 && y - 1 >= 0)
                if (BoardTab[x - 1, y - 1].Type != CellType.Empty)
                    numOfNeighbors++;

            if (x + 1 < BoardSize && y - 1 >= 0)
                if (BoardTab[x + 1, y - 1].Type != CellType.Empty)
                    numOfNeighbors++;

            if (x + 1 < BoardSize && y + 1 < BoardSize)
                if (BoardTab[x + 1, y + 1].Type != CellType.Empty)
                    numOfNeighbors++;

            if (x - 1 >= 0 && y + 1 < BoardSize)
                if (BoardTab[x - 1, y + 1].Type != CellType.Empty)
                    numOfNeighbors++;
        }

        return numOfNeighbors;
    }

    /// <summary>
    /// Metoda pomocnicza (<see cref="RandomNeighbors(int)"/>) tworząca pokój na podstawie prawdopodobieństwa i ilości sąsiadów dla danego pola.
    /// </summary>
<<<<<<< HEAD:Assets/Scripts/Classes/Board.cs
    /// <param name="x">Indeks wiersza tabeli.</param>
    /// <param name="y">Indeks kolumny tabeli.</param>
    /// <returns>Informacja czy pokój został stworzony.</returns>
=======
    /// <param name="x">Indeks kolumny tabeli.</param>
    /// <param name="y">Indeks wiersza tabeli.</param>
    /// <returns>Metoda zwraca informację czy pokój został stworzony.</returns>
>>>>>>> c988aed30ab75eb3052d2af5d49c8fc66b18cec8:Assets/Scripts/Board/Board.cs
    private bool MakeRoom(int x, int y)
    {
        bool hasCloseNeighbours;
        int numOfNeighbors = NumberOfNeighbors(x, y, out hasCloseNeighbours);
        var percent = _rand.Next(0, 101);
        bool isMaked = false;

        if (BoardTab[x, y].Type != CellType.Empty)
            return isMaked;

        if (!hasCloseNeighbours)
            return isMaked;

        switch (numOfNeighbors)
        {
            case 0:
                break;
            case 1:
                if (percent <= 50)
                {
                    PlaceRoom(x, y, CellType.Common);
                    isMaked = true;
                }
                break;
            case 2:
                if (percent <= 40)
                {
                    PlaceRoom(x, y, CellType.Common);
                    isMaked = true;
                }
                break;
            case 3:
                if (percent <= 10)
                {
                    PlaceRoom(x, y, CellType.Common);
                    isMaked = true;
                }
                break;
            //case 4:
            //    if (percent <= 5)
            //    {
            //        BoardTab[x, y] = (int)Cell.Common;
            //        isMaked = true;
            //    }
            //    break;
            default:
                break;
        }

        return isMaked;
    }

    /// <summary>
    /// Metoda pomocnicza (<see cref="FillBoard(string)"/>) tworząca unikalne pokoje.
    /// </summary>
    /// <param name="roomOffset">Indeks w <see cref="CellType"/> pierwszego z unikalnych pokoi.</param>
    /// <param name="roomCoreCount">Ilość unikalnych pokoi.</param>
    private void MakeCoreRoom(int roomOffset, int roomCoreCount)
    {
        List<Coords> singleRoomList = new List<Coords>();
        Coords cell = new Coords(0, 0);
        int numOfNeighbours = 0;
        int index = 0;
        bool trash;

        for (int i = 0; i < BoardSize; i++)
            for (int j = 0; j < BoardSize; j++)
            {
                if (BoardTab[i, j].Type == CellType.Common)
                {
                    numOfNeighbours = NumberOfNeighbors(i, j, out trash, true);
                    if (numOfNeighbours == 1)
                        singleRoomList.Add(new Coords(i, j));
                }
            }

        int listCount = singleRoomList.Count;

        if (singleRoomList.Count > 0)
            for (int i = 0; i < listCount && i < roomCoreCount; i++)
            {
                index = _rand.Next(0, singleRoomList.Count);
                cell = singleRoomList[index];
                singleRoomList.RemoveAt(index);
                PlaceRoom(cell.X, cell.Y, (CellType)(i + roomOffset));
            }
        else
        {
            for (int i = 0; i < BoardSize; i++)
                for (int j = 0; j < BoardSize; j++)
                {
                    if (BoardTab[i, j].Type == CellType.Empty)
                    {
                        numOfNeighbours = NumberOfNeighbors(i, j, out trash, true);
                        if (numOfNeighbours == 1)
                        {
                            PlaceRoom(i, j, CellType.Boss);
                            //Wychodzimy z obu pętli
                            i = BoardSize;
                            break;
                        }
                    }
                }
        }
    }

    /// <summary>
    /// Metoda do umieszczania komórki w tabeli z aktualizacją zmiennych dla sąsiednich komórek.
    /// </summary>
    /// <param name="x">Indeks kolumny tabeli.</param>
    /// <param name="y">Indeks wiersza tabeli.</param>
    /// <param name="type"></param>
    public void PlaceRoom(int x, int y, CellType type)
    {
        BoardTab[x, y].Type = type;

        if (y - 1 >= 0)
        {
            BoardTab[x, y].BottomNeighbour = BoardTab[x, y - 1].Type;
            BoardTab[x, y - 1].TopNeighbour = type;
        }
        if (y + 1 < BoardSize)
        {
            BoardTab[x, y].TopNeighbour = BoardTab[x, y + 1].Type;
            BoardTab[x, y + 1].BottomNeighbour = type;
        }

        if (x + 1 < BoardSize)
        {
            BoardTab[x, y].RightNeighbour = BoardTab[x + 1, y].Type;
            BoardTab[x + 1, y].LeftNeighbour = type;
        }
        if (x - 1 >= 0)
        {
            BoardTab[x, y].LeftNeighbour = BoardTab[x - 1, y].Type;
            BoardTab[x - 1, y].RightNeighbour = type;
        }
    }
}

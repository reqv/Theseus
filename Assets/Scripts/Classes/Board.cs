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
        /// Współrzędna X
        /// </summary>
        public int X
        {
            get; set;
        }
        /// <summary>
        /// Współrzędna Y
        /// </summary>
        public int Y
        {
            get; set;
        }

        /// <summary>
        /// Konstruktor klasy Coords przyjmujący dwie wartości.
        /// </summary>
        /// <param name="x">Współrzędna X</param>
        /// <param name="y">Współrzędna Y</param>
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Enumerator odpowiadający za rodzaje pokoi (komórek na planszy).
    /// </summary>
    private enum Cell
    {
        Empty = 0,
        Common = 1,
        Start = 2,
        Boss = 3,
        Premium = 4,
        Shop = 5

    }

    /// <summary>
    /// Obiekt klasy Random odpowiadający za losowanie.
    /// </summary>
    private Random _rand;
    /// <summary>
    /// Minimalna ilość pokoi.
    /// </summary>
    public int MinRoomCount
    {
        get
        {
            return MinRoomCount;
        }
        set
        {
            value = 15;
        }
    }
    /// <summary>
    /// Maksymalna ilość pokoi.
    /// </summary>
    public int MaxRoomCount
    {
        get
        {
            return MaxRoomCount;
        }
        set
        {
            value = 20;
        }
    }

    private int _boardSize;
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
            return _boardSize;
        }
        set
        {
            HalfOfBoardSize = value / 2;
            _boardSize = value;
        }
    }

    /// <summary>
    /// Tablica zawierająca informację o rozmieszczeniu pokoi.
    /// </summary>
    public int[,] BoardTab
    {
        get;
        set;
    }

    /// <summary>
    /// Połowa rozmiaru planszy.
    /// </summary>
    public int HalfOfBoardSize
    {
        get;
        private set;
    }

    /// <summary>
    /// Konstruktor klasy Board określający rozmiar planszy.
    /// </summary>
    /// <param name="boardSize">Rozmiar planszy.</param>
    public Board(int boardSize)
    {
        BoardTab = new int[boardSize, boardSize];
        BoardSize = boardSize;
    }

    /// <summary>
    /// Metoda odpowiedzialna za wyczyszczenie planszy (wypełnia komórki wartością 0).
    /// </summary>
    /// <param name="boardSize">Rozmiar planszy.</param>
    /// <param name="board">Czyszczona plansza.</param>
    public void ResetBoard(int boardSize, int[,] board)
    {
        for (int i = 0; i < boardSize; i++)
            for (int j = 0; j < boardSize; j++)
                board[i, j] = 0;
    }

    /// <summary>
    /// Główna metoda odpowiedzialna za wygenerowanie rozmieszczenia pokoi.
    /// </summary>
    /// <param name="seed">Ziarno losowości (domyślnie pusty). Jeśli puste generuje nowe ziarno.</param>
    /// <returns>Zwraca ziarno rozmieszczenia pokoi.</returns>
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

        BoardTab[HalfOfBoardSize, HalfOfBoardSize] = (int)Cell.Start;
        RandomNeighbors(1);
        MakeCoreRoom(roomOffset, roomCoreCount);

        return seed;
    }

    /// <summary>
    /// Metoda pomocnicza odpowiadająca za ilość sąsiadów danej komórki.
    /// </summary>
    /// <remarks>
    /// Metoda pomocniczna wykorzystywana w metodzie <see cref="FillBoard(string)"./>
    /// </remarks>
    /// <param name="roomCounter">Liczba pokoi znajdujących się już w tabeli.</param>
    private void RandomNeighbors(int roomCounter)
    {
        bool isRandomized = false;

        int iLowerLimit = 3, jLowerLimit = 3, iUpperLimit = 3, jUpperLimit = 3;
        do
        {
            for (int i = iLowerLimit; i < BoardSize - iUpperLimit; i++)
            {
                for (int j = jLowerLimit; j < BoardSize - jUpperLimit; j++)
                {
                    if (MakeRoom(i, j))
                    {
                        roomCounter++;
                        if (i == iLowerLimit)
                            iLowerLimit = iLowerLimit == 0 ? 0 : iLowerLimit - 1;
                        else if (i == iUpperLimit)
                            iUpperLimit = iUpperLimit == 0 ? 0 : iUpperLimit - 1;
                        if (j == jLowerLimit)
                            jLowerLimit = jLowerLimit == 0 ? 0 : jLowerLimit - 1;
                        else if (j == jUpperLimit)
                            jUpperLimit = jUpperLimit == 0 ? 0 : jUpperLimit - 1;
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
    /// Metoda pomocnicza zliczająca sąsiadów danej komórki tabeli.
    /// </summary>
    /// <param name="x">Indeks wiersza tabeli.</param>
    /// <param name="y">Indeks kolumny tabeli.</param>
    /// <param name="hasCloseNeighbours">Parametr typu out zwracający nam informację o tym czy dana komórka ma sąsiada w kierunku N/E/S/W.</param>
    /// <param name="checkOnlyCloseNeighbours">Parametr domyślny (false) decydujący o tym czy mają być sprawdzani wszyscy sąsiedzi wokół komórki czy tylko w podstawowych kierunkach N/E/S/W.</param>
    /// <returns>Metoda zwraca ilość sąsiadów</returns>
    private int NumberOfNeighbors(int x, int y, out bool hasCloseNeighbours, bool checkOnlyCloseNeighbours = false)
    {
        hasCloseNeighbours = false;
        int numOfNeighbors = 0;

        if (y - 1 >= 0)
            if (BoardTab[x, y - 1] != (int)Cell.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (x + 1 < BoardSize)
            if (BoardTab[x + 1, y] != (int)Cell.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (y + 1 < BoardSize)
            if (BoardTab[x, y + 1] != (int)Cell.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (x - 1 >= 0)
            if (BoardTab[x - 1, y] != (int)Cell.Empty)
            {
                numOfNeighbors++;
                hasCloseNeighbours = true;
            }

        if (!checkOnlyCloseNeighbours)
        {
            if (x - 1 >= 0 && y - 1 >= 0)
                if (BoardTab[x - 1, y - 1] != (int)Cell.Empty)
                    numOfNeighbors++;

            if (x + 1 < BoardSize && y - 1 >= 0)
                if (BoardTab[x + 1, y - 1] != (int)Cell.Empty)
                    numOfNeighbors++;

            if (x + 1 < BoardSize && y + 1 < BoardSize)
                if (BoardTab[x + 1, y + 1] != (int)Cell.Empty)
                    numOfNeighbors++;

            if (x - 1 >= 0 && y + 1 < BoardSize)
                if (BoardTab[x - 1, y + 1] != (int)Cell.Empty)
                    numOfNeighbors++;
        }

        return numOfNeighbors;
    }

    /// <summary>
    /// Metoda pomocnicza tworząca pokój na podstawie prawdopodobieństwa i ilości sąsiadów dla danego pola.
    /// </summary>
    /// <param name="x">Indeks wiersza tabeli.</param>
    /// <param name="y">Indeks kolumny tabeli.</param>
    /// <returns>Metoda zwraca informację czy pokój został stworzony.</returns>
    private bool MakeRoom(int x, int y)
    {
        bool hasCloseNeighbours;
        int numOfNeighbors = NumberOfNeighbors(x, y, out hasCloseNeighbours);
        var percent = _rand.Next(0, 101);
        bool isMaked = false;

        if (BoardTab[x, y] != (int)Cell.Empty)
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
                    BoardTab[x, y] = (int)Cell.Common;
                    isMaked = true;
                }
                break;
            case 2:
                if (percent <= 40)
                {
                    BoardTab[x, y] = (int)Cell.Common;
                    isMaked = true;
                }
                break;
            case 3:
                if (percent <= 10)
                {
                    BoardTab[x, y] = (int)Cell.Common;
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
    /// Metoda tworząca unikalne pokoje.
    /// </summary>
    /// <param name="roomOffset">Indeks w <see cref="Cell"/> pierwszego z unikalnych pokoi.</param>
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
                if (BoardTab[i, j] == (int)Cell.Common)
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
                BoardTab[cell.X, cell.Y] = i + roomOffset;
            }
        else
        {
            for (int i = 0; i < BoardSize; i++)
                for (int j = 0; j < BoardSize; j++)
                {
                    if (BoardTab[i, j] == (int)Cell.Empty)
                    {
                        numOfNeighbours = NumberOfNeighbors(i, j, out trash, true);
                        if (numOfNeighbours == 1)
                        {
                            BoardTab[i, j] = (int)Cell.Boss;
                            //Wychodzimy z obu pętli
                            i = BoardSize;
                            break;
                        }
                    }
                }
        }
    }

}

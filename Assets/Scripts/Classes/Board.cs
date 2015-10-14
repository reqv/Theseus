using System;
using System.Collections.Generic;
using System.Linq;

public class Board
{
    private class Coords
    {
        public int X
        {
            get; set;
        }
        public int Y
        {
            get; set;
        }

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    private enum Cell
    {
        Empty = 0,
        Common = 1,
        Start = 2,
        Boss = 3,
        Premium = 4,
        Shop = 5

    }
    private Random _rand;
    private int _minRoomCount = 15;
    private int _maxRoomCount = 20;

    private int _boardSize;
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
    public int[,] BoardTab
    {
        get;
        set;
    }
    public int HalfOfBoardSize
    {
        get;
        private set;
    }

    public Board(int boardSize)
    {
        BoardTab = new int[boardSize, boardSize];
        BoardSize = boardSize;
    }

    public void ResetBoard(int boardSize, int[,] board)
    {
        for (int i = 0; i < boardSize; i++)
            for (int j = 0; j < boardSize; j++)
                board[i, j] = 0;
    }

    public void FillBoard(string seed = "")
    {
        const int roomOffset = 3;
        const int roomCoreCount = 3;
        const int seedLength = 6;

        BoardTab[HalfOfBoardSize, HalfOfBoardSize] = (int)Cell.Start;
        if (string.IsNullOrEmpty(seed))
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomr = new Random();
            seed = new string(Enumerable.Repeat(chars, seedLength).Select(s => s[randomr.Next(s.Length)]).ToArray());
        }

        _rand = new Random(seed.GetHashCode());
        RandomNeighbors(1);
        MakeCoreRoom(roomOffset, roomCoreCount);
    }

    private void RandomNeighbors(int roomCounter, bool isStartingRoom = true)
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
                    if (roomCounter >= _minRoomCount)
                    {
                        isRandomized = true;
                    }
                    if (roomCounter >= _maxRoomCount)
                        break;
                }
                if (roomCounter >= _maxRoomCount)
                    break;
            }
        } while (!isRandomized);
    }

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

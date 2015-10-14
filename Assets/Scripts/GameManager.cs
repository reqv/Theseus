using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private const int _boardSize = 9;
    private Board _board = new Board(_boardSize);
    private Vector2 _actualPosition;

    // Use this for initialization
    void Start ()
    {
        _board.FillBoard();
        _actualPosition = new Vector2(_board.HalfOfBoardSize, _board.HalfOfBoardSize);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

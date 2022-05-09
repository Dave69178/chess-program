using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateAllPieces
{
    public static List<int> GetAllPieceSquares(Board board)
    {
        List<int> pieceSquares = new List<int>();
        for (int i = 0; i < 64; i++)
        {
            if (board.boardArray[i] != Pieces.None)
            {
                pieceSquares.Add(i);
            }
        }
        return pieceSquares;
    }
}

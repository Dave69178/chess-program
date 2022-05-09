using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePieces
{
    public static List<int> GetActivePieceSquares(Board board)
    {
        int activeColour = board.GetActiveColourAsInt();

        List<int> activePieceSquares = new List<int>();
        for (int i = 0; i < 64; i++)
        {
            if ((board.boardArray[i] & activeColour) == activeColour)
            {
                activePieceSquares.Add(i);
            }
        }
        return activePieceSquares;
    }

    public static List<int> GetActivePieces(Board board)
    {
        int activeColour;
        if (board.activeColour == 'w')
        {
            activeColour = Pieces.White;
        }
        else
        {
            activeColour = Pieces.Black;
        }

        List<int> activePieces = new List<int>();
        for (int i = 0; i < 64; i++)
        {
            if ((board.boardArray[i] & activeColour) == activeColour)
            {
                activePieces.Add(board.boardArray[i]);
            }
        }
        return activePieces;
    }
}

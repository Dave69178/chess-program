using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CastleMoves
{
    //  All to be called if respective castling rights on board are true
    public static List<int> WhiteCastleKingside(List<int> kingMoves, Board board)
    {
        if (!CheckDetection.IsSquareAttacked(board, 60))
        {
            if (board.boardArray[61] == Pieces.None && board.boardArray[62] == Pieces.None)
            {
                if (!(CheckDetection.IsSquareAttacked(board, 61) && CheckDetection.IsSquareAttacked(board, 62)))
                {
                    kingMoves.Add(62);
                }
            }
        }
        return kingMoves;
    }

    public static List<int> WhiteCastleQueenside(List<int> kingMoves, Board board)
    {
        if (!CheckDetection.IsSquareAttacked(board, 60))
        {
            if (board.boardArray[59] == Pieces.None && board.boardArray[58] == Pieces.None && board.boardArray[57] == Pieces.None)
            {
                if (!(CheckDetection.IsSquareAttacked(board, 59) && CheckDetection.IsSquareAttacked(board, 58) &&
                    CheckDetection.IsSquareAttacked(board, 57)))
                {
                    kingMoves.Add(58);
                }
            }
        }
        return kingMoves;
    }

    public static List<int> BlackCastleKingside(List<int> kingMoves, Board board)
    {
        if (!CheckDetection.IsSquareAttacked(board, 4))
        {
            if (board.boardArray[5] == Pieces.None && board.boardArray[6] == Pieces.None)
            {
                if (!(CheckDetection.IsSquareAttacked(board, 5) && CheckDetection.IsSquareAttacked(board, 6)))
                {
                    kingMoves.Add(6);
                }
            }
        }
        return kingMoves;
    }

    public static List<int> BlackCastleQueenside(List<int> kingMoves, Board board)
    {
        if (!CheckDetection.IsSquareAttacked(board, 4))
        {
            if (board.boardArray[3] == Pieces.None && board.boardArray[2] == Pieces.None && board.boardArray[1] == Pieces.None)
            {
                if (!(CheckDetection.IsSquareAttacked(board, 3) && CheckDetection.IsSquareAttacked(board, 2) &&
                    CheckDetection.IsSquareAttacked(board, 1)))
                {
                    kingMoves.Add(2);
                }
            }
        }
        return kingMoves;
    }
}
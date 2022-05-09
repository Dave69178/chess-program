using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LegalMoves
{
    public static List<int> RemoveIllegalMovesToCheck(Board board, int square, List<int> moves)
    {
        int pieceOnMoveSquareHolder;
        int squareToMoveToHolder;
        int activeColour = board.GetActiveColourAsInt();

        foreach (int move in moves.ToList())
        {
            //  Save original position
            pieceOnMoveSquareHolder = board.boardArray[square];
            squareToMoveToHolder = board.boardArray[move];

            //  Update position with move
            board.boardArray[move] = board.boardArray[square];
            board.boardArray[square] = Pieces.None;

            //  Find own King
            int kingSquare = Array.IndexOf(board.boardArray, Pieces.King | activeColour);

            //  Check if King is attacked, if yes remove move
            if (CheckDetection.IsSquareAttacked(board, kingSquare))
            {
                moves.Remove(move);
            }

            //  Revert to original position
            board.boardArray[square] = pieceOnMoveSquareHolder;
            board.boardArray[move] = squareToMoveToHolder;
        }

        return moves;
    }

    public static List<int> GetLegalMovesForSquare(Board board, int square)
    {
        //  Generate psuedo-legal moves
        List<int> moves = PsuedoLegalMoveGenerators.FindMovesForGenericPiece(board, square);

        //  Remove illegal moves to check
        return RemoveIllegalMovesToCheck(board, square, moves);
    }
}

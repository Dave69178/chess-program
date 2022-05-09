using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CheckDetection
{
    //  Function to check if a square is attacked
    public static bool IsSquareAttacked (Board board, int square)
    {
        int colour = board.GetActiveColourAsInt();
        int opponentColour = colour == Pieces.White ? Pieces.Black : Pieces.White;
        (int col, int row) = BoardHelperFunctions.GetIndexFromSquare(square);

        int forwardLeftSquare;
        int forwardRightSquare;
        int leftMostColumn;
        if (colour == Pieces.White)
        {
            forwardLeftSquare = OneSquare.northWest;
            forwardRightSquare = OneSquare.northEast;
            leftMostColumn = 0;
        }
        else
        {
            forwardLeftSquare = OneSquare.southEast;
            forwardRightSquare = OneSquare.southWest;
            leftMostColumn = 7;
        }

        //  Check if opponent pawns attack square
        if (col == leftMostColumn)  //  Special cases for square in edge columns
        {
            if (board.boardArray[square + forwardRightSquare] == (Pieces.Pawn | opponentColour))
            {
                return true;
            }
        }
        else if (col == 7 - leftMostColumn)
        {
            if (board.boardArray[square + forwardLeftSquare] == (Pieces.Pawn | opponentColour))
            {
                return true;
            }
        }
        else  //  When square is not in edge column
        {
            if (board.boardArray[square + forwardRightSquare] == (Pieces.Pawn | opponentColour) || board.boardArray[square + forwardLeftSquare] == (Pieces.Pawn | opponentColour))
            {
                return true;
            }
        }

        //  Check if opponent's King attacks square
        foreach (int adjacentSquare in OneSquare.GetSurroundingSquares(square))
        {
            if (board.boardArray[square + adjacentSquare] == (Pieces.King | opponentColour))
            {
                return true;
            }
        }

        //  Check if attacked by opponent Knight
        List<int> knightAttacks = PsuedoLegalMoveGenerators.KnightMoves(board, square);
        foreach (int move in knightAttacks)
        {
            if (board.boardArray[move] == (Pieces.Knight | opponentColour))
            {
                return true;
            }
        }

        //  Check if attacked by opponent Rook or Queen
        List<int> rookAttacks = PsuedoLegalMoveGenerators.RookMoves(board, square);
        foreach (int move in rookAttacks)
        {
            if (board.boardArray[move] == (Pieces.Rook | opponentColour) || board.boardArray[move] == (Pieces.Queen | opponentColour))
            {
                return true;
            }
        }

        //  Check if attacked by opponent Bishop or Queen
        List<int> bishopAttacks = PsuedoLegalMoveGenerators.BishopMoves(board, square);
        foreach (int move in bishopAttacks)
        {
            if (board.boardArray[move] == (Pieces.Bishop | opponentColour) || board.boardArray[move] == (Pieces.Queen | opponentColour))
            {
                return true;
            }
        }

        //  Otherwise, square not attacked
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class FenString
{
    public static Board FenStringToBoard (string fen)
    {
        Board board = new Board ();

        string[] fenFields = fen.Split(' ');

        //  Set board array
        board.boardArray = FenPiecePlacementToBoardArray(fenFields[0]);

        //  Set active colour
        board.activeColour = fenFields[1][0];

        //  Set castle rights
        if (fenFields[2].Contains("K"))
        {
            board.whiteKingsideCastle = true;
        }
        else
        {
            board.whiteKingsideCastle = false;
        }

        if (fenFields[2].Contains("Q"))
        {
            board.whiteQueensideCastle = true;
        }
        else
        {
            board.whiteQueensideCastle = false;
        }

        if (fenFields[2].Contains("k"))
        {
            board.blackKingsideCastle = true;
        }
        else
        {
            board.blackKingsideCastle = false;
        }

        if (fenFields[2].Contains("q"))
        {
            board.blackQueensideCastle = true;
        }
        else
        {
            board.blackQueensideCastle = false;
        }

        //  Set En Passant square if legal
        if (fenFields[3] == "-")
        {
            board.enPassantSquare = -1;  //  Indicating no En Passant possible
        }
        else
        {
            board.enPassantSquare = AlgebraicNotationConversion.AlgebraicSquareToInt(fenFields[3]);
        }

        //  Set Half move clock
        board.halfMoveClock = 0;

        //  Set full move number
        board.fullMoveNumber = 1;

        //  Set game state
        if (EndConditions.IsCheckMate(board))
        {
            if (board.activeColour == 'w')
            {
                board.gameState = Board.State.WhiteIsMated;
            }
            else
            {
                board.gameState = Board.State.BlackIsMated;
            }
        }
        else if (EndConditions.IsStaleMate(board))
        {
            board.gameState = Board.State.StaleMate;
        } 
        else if (EndConditions.IsInsufficientMaterial(board))
        {
            board.gameState = Board.State.InsufficientMaterial;
        }
        else if (EndConditions.IsFiftyMoveRule(board))
        {
            board.gameState = Board.State.FiftyMoveRule;
        }
        else
        {
            board.gameState = Board.State.Playing;
        }

        //  Initialise game history
        board.gameHistory = new List<string> { BoardToFenString(board) };

        return board;
    }

    public static string BoardToFenString (Board board)
    {
        string[] fenFields = new string[6];

        fenFields[0] = BoardArrayToFenStringPlacement(board.boardArray);

        fenFields[1] = board.activeColour.ToString();

        fenFields[2] = "";
        if (board.whiteKingsideCastle)
        {
            fenFields[2] += "K";
        }
        if (board.whiteQueensideCastle)
        {
            fenFields[2] += "Q";
        }
        if (board.blackKingsideCastle)
        {
            fenFields[2] += "k";
        }
        if (board.blackQueensideCastle)
        {
            fenFields[2] += "q";
        }
        if (fenFields[2] == "")
        {
            fenFields[2] += "-";
        }

        if (board.enPassantSquare != -1)
        {
            fenFields[3] = AlgebraicNotationConversion.IntToAlgebraicSquare(board.enPassantSquare);
        }
        else
        {
            fenFields[3] = "-";
        }

        fenFields[4] = board.halfMoveClock.ToString();

        fenFields[5] = board.fullMoveNumber.ToString();

        return String.Join(" ", fenFields);
    }

    public static int[] FenPiecePlacementToBoardArray (string fenPiecePlacement)
    {
        int[] boardArray = new int[64];
        int square = 0;

        for (int i = 0; i < fenPiecePlacement.Length; i++)
        {
            if (char.IsLetter(fenPiecePlacement[i]))
            {
                boardArray[square] = AlgebraicNotationConversion.AlgebraicToBoardPieces[fenPiecePlacement[i]];
                square++;
            }
            else if (char.IsDigit(fenPiecePlacement[i]))
            {
                for (int j = 0; j < fenPiecePlacement[i] - '0'; j++)
                {
                    boardArray[square + j] = Pieces.None;
                }
                square += fenPiecePlacement[i] - '0';
            }
            else
            {
                continue;
            }
        }
        return boardArray;
    }

    public static string BoardArrayToFenStringPlacement (int[] boardArray)
    {
        string fenPiecePlacment = "";
        int emptySquareCount = 0;

        for (int square = 0; square < 64; square++)
        {
            if (boardArray[square] == Pieces.None)
            {
                emptySquareCount++;
            }
            else
            {
                if (emptySquareCount != 0)
                {
                    fenPiecePlacment += emptySquareCount;
                }
                emptySquareCount = 0;
                fenPiecePlacment += AlgebraicNotationConversion.BoardPiecesToAlgrebaric[boardArray[square]];
            }

            if ((square + 1) % 8 == 0 && square != 63)
            {
                if (emptySquareCount != 0)
                {
                    fenPiecePlacment += emptySquareCount;
                }
                emptySquareCount = 0;
                fenPiecePlacment += "/";
            }
        }
        return fenPiecePlacment;
    }

    
}

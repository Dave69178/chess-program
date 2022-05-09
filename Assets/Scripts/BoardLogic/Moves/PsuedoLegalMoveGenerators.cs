using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class PsuedoLegalMoveGenerators
{
    public static List<int> FindMovesForGenericPiece (Board board, int square)
    {
        int[] boardArray = board.boardArray;
        if (boardArray[square] == (Pieces.Pawn | Pieces.White) || boardArray[square] == (Pieces.Pawn | Pieces.Black))
        {
            return PawnMoves(board, square);
        }
        else if (boardArray[square] == (Pieces.King | Pieces.White) || boardArray[square] == (Pieces.King | Pieces.Black))
        {
            return KingMoves(board, square);
        }
        else if (boardArray[square] == (Pieces.Knight | Pieces.White) || boardArray[square] == (Pieces.Knight | Pieces.Black))
        {
            return KnightMoves(board, square);
        }
        else if (boardArray[square] == (Pieces.Bishop | Pieces.White) || boardArray[square] == (Pieces.Bishop | Pieces.Black))
        {
            return BishopMoves(board, square);
        }
        else if (boardArray[square] == (Pieces.Rook | Pieces.White) || boardArray[square] == (Pieces.Rook | Pieces.Black))
        {
            return RookMoves(board, square);
        }
        else if (boardArray[square] == (Pieces.Queen | Pieces.White) || boardArray[square] == (Pieces.Queen | Pieces.Black))
        {
            return QueenMoves(board, square);
        }
        else
        {
            return null;
        }
    }

    public static List<int> PawnMoves(Board board, int square)
    {
        int colour = board.GetActiveColourAsInt();
        List<int> moves = new List<int>();
        (int col, int row) = BoardHelperFunctions.GetIndexFromSquare(square);
        int opponentColour = colour == Pieces.White ? Pieces.Black : Pieces.White;
        int startRow;
        int forwardOneSquare;
        int forwardLeftSquare; //  From their perspective (black attacks higher file, white attacks lower file)
        int forwardRightSquare;
        int leftMostColumn;  //  Again from perspective of colour (0 for white, 7 for black)

        if (colour == Pieces.White)
        {
            startRow = 6;
            forwardOneSquare = OneSquare.north;
            forwardLeftSquare = OneSquare.northWest;
            forwardRightSquare = OneSquare.northEast;
            leftMostColumn = 0;
        }
        else
        {
            startRow = 1;
            forwardOneSquare = OneSquare.south;
            forwardLeftSquare = OneSquare.southEast;
            forwardRightSquare = OneSquare.southWest;
            leftMostColumn = 7;
        }

        //  If square infront is empty, add move.
        if (board.boardArray[square + forwardOneSquare] == Pieces.None)
        {
            moves.Add(square + forwardOneSquare);
            //  If pawn hasn't moved and square two infront is empty as well add move.
            if (row == startRow && (board.boardArray[square + forwardOneSquare * 2] == Pieces.None))
            {
                moves.Add(square + forwardOneSquare * 2);
            }
        }

        //  If pawn attacks enemy piece add move.
        //  Remove pawns on edge from taking on other side of board.
        if (col != leftMostColumn && (board.boardArray[square + forwardLeftSquare] & opponentColour) == opponentColour)
        {
            moves.Add(square + forwardLeftSquare);
        }

        if (col != (7 - leftMostColumn) && (board.boardArray[square + forwardRightSquare] & opponentColour) == opponentColour)
        {
            moves.Add(square + forwardRightSquare);
        }

        //  Add En Passant capture if possible
        if (board.enPassantSquare > - 1)
        {
            if (square + forwardLeftSquare == board.enPassantSquare || square + forwardRightSquare == board.enPassantSquare)
            {
                moves.Add(board.enPassantSquare);
            }
        }

        return moves;
    }

    public static List<int> KingMoves(Board board, int square)
    {
        int colour = board.GetActiveColourAsInt();
        List<int> moves = new List<int> { square + OneSquare.north, square + OneSquare.northEast, square + OneSquare.east, square + OneSquare.southEast, square + OneSquare.south, square + OneSquare.southWest, square + OneSquare.west, square + OneSquare.northWest };
        (int col, int row) = BoardHelperFunctions.GetIndexFromSquare(square);
        int opponentColour = colour == Pieces.White ? Pieces.Black : Pieces.White;

        //  Remove moves to other side of board if on edge
        if (col == 0)
        {
            moves.Remove(square + OneSquare.northWest);
            moves.Remove(square + OneSquare.west);
            moves.Remove(square + OneSquare.southWest);
        }
        else if (col == 7)
        {
            moves.Remove(square + OneSquare.northEast);
            moves.Remove(square + OneSquare.east);
            moves.Remove(square + OneSquare.southEast);
        }

        if (row == 0)
        {
            moves.Remove(square + OneSquare.northWest);
            moves.Remove(square + OneSquare.north);
            moves.Remove(square + OneSquare.northEast);
        }
        else if (row == 7)
        {
            moves.Remove(square + OneSquare.southWest);
            moves.Remove(square + OneSquare.south);
            moves.Remove(square + OneSquare.southEast);
        }

        //  Check for empty squares or opponents pieces and add moves
        foreach (int move in moves.ToList())
        {
            if (!(board.boardArray[move] == Pieces.None || (board.boardArray[move] & opponentColour) == opponentColour))
            {
                moves.Remove(move);
            }
        }

        //  Add castling moves depending upon rights
        if (colour == Pieces.White)
        {
            if (board.whiteKingsideCastle)
            {
                moves.AddRange(CastleMoves.WhiteCastleKingside(moves, board));
            }

            if (board.whiteQueensideCastle)
            {
                moves.AddRange(CastleMoves.WhiteCastleQueenside(moves, board));
            }
        }
        else
        {
            if (board.blackKingsideCastle)
            {
                moves.AddRange(CastleMoves.BlackCastleKingside(moves, board));
            }

            if (board.blackQueensideCastle)
            {
                moves.AddRange(CastleMoves.BlackCastleQueenside(moves, board));
            }
        }

        return moves;
    }

    public static List<int> KnightMoves(Board board, int square)
    {
        int colour = board.GetActiveColourAsInt();
        List<int> moves = new List<int> { square + OneSquare.north * 2 + OneSquare.east, square + OneSquare.north * 2 + OneSquare.west, square + OneSquare.east * 2 + OneSquare.north,
                                        square + OneSquare.east * 2 + OneSquare.south, square + OneSquare.south * 2 + OneSquare.east, square + OneSquare.south * 2 + OneSquare.west,
                                        square + OneSquare.west * 2 + OneSquare.south, square + OneSquare.west * 2 + OneSquare.north };
        (int col, int row) = BoardHelperFunctions.GetIndexFromSquare(square);
        int opponentColour = colour == Pieces.White ? Pieces.Black : Pieces.White;

        //  Remove moves that cross edge of board
        if (col == 0)
        {
            moves.Remove(square + OneSquare.north * 2 + OneSquare.west);
            moves.Remove(square + OneSquare.south * 2 + OneSquare.west);
            moves.Remove(square + OneSquare.west * 2 + OneSquare.south);
            moves.Remove(square + OneSquare.west * 2 + OneSquare.north);
        }
        else if (col == 1)
        {
            moves.Remove(square + OneSquare.west * 2 + OneSquare.south);
            moves.Remove(square + OneSquare.west * 2 + OneSquare.north);
        }
        else if (col == 6)
        {
            moves.Remove(square + OneSquare.east * 2 + OneSquare.south);
            moves.Remove(square + OneSquare.east * 2 + OneSquare.north);
        }
        else if (col == 7)
        {
            moves.Remove(square + OneSquare.north * 2 + OneSquare.east);
            moves.Remove(square + OneSquare.south * 2 + OneSquare.east);
            moves.Remove(square + OneSquare.east * 2 + OneSquare.south);
            moves.Remove(square + OneSquare.east * 2 + OneSquare.north);
        }

        if (row == 0)
        {
            moves.Remove(square + OneSquare.north * 2 + OneSquare.west);
            moves.Remove(square + OneSquare.north * 2 + OneSquare.east);
            moves.Remove(square + OneSquare.west * 2 + OneSquare.north);
            moves.Remove(square + OneSquare.east * 2 + OneSquare.north);
        }
        else if (row == 1)
        {
            moves.Remove(square + OneSquare.north * 2 + OneSquare.west);
            moves.Remove(square + OneSquare.north * 2 + OneSquare.east);
        }
        else if (row == 6)
        {
            moves.Remove(square + OneSquare.south * 2 + OneSquare.west);
            moves.Remove(square + OneSquare.south * 2 + OneSquare.east);
        }
        else if (row == 7)
        {
            moves.Remove(square + OneSquare.south * 2 + OneSquare.west);
            moves.Remove(square + OneSquare.south * 2 + OneSquare.east);
            moves.Remove(square + OneSquare.west * 2 + OneSquare.south);
            moves.Remove(square + OneSquare.east * 2 + OneSquare.south);
        }

        //  Check for empty squares or opponents pieces and add moves
        foreach (int move in moves.ToList())
        {
            if (!(board.boardArray[move] == Pieces.None || (board.boardArray[move] & opponentColour) == opponentColour))
            {
                moves.Remove(move);
            }
        }

        return moves;
    }

    public static List<int> BishopMoves(Board board, int square)
    {
        int colour = board.GetActiveColourAsInt();
        List<int> moves = new List<int>();
        (int col, int row) = BoardHelperFunctions.GetIndexFromSquare(square);
        int opponentColour = colour == Pieces.White ? Pieces.Black : Pieces.White;

        //  Determine maximum number of moves along each diagonal direction
        int northEastCount = Math.Min(7 - col, row);
        int northWestCount = Math.Min(col, row);
        int southEastCount = Math.Min(7 - col, 7 - row);
        int southWestCount = Math.Min(col, 7 - row);
        int[] diagonalDirectionLengths = new int[4] { northEastCount, northWestCount, southEastCount, southWestCount };

        //  Add moves until end of diagonal unless a piece can be captured, or own piece blocks diagonal.
        int count = 0;
        int diagonalOneSquare;
        foreach (int diagonalLength in diagonalDirectionLengths)
        {
            if (count == 0)
            {
                diagonalOneSquare = OneSquare.northEast;
            }
            else if (count == 1)
            {
                diagonalOneSquare = OneSquare.northWest;
            }
            else if (count == 2)
            {
                diagonalOneSquare = OneSquare.southEast;
            }
            else
            {
                diagonalOneSquare = OneSquare.southWest;
            }

            for (int i = 0; i < diagonalLength; i++)
            {
                if (board.boardArray[square + diagonalOneSquare * (i + 1)] == Pieces.None)  //  If empty space, add move
                {
                    moves.Add(square + diagonalOneSquare * (i + 1));
                }
                else if ((board.boardArray[square + diagonalOneSquare * (i + 1)] & opponentColour) == opponentColour) //  If opponent piece, add move and break
                {
                    moves.Add(square + diagonalOneSquare * (i + 1));
                    break;
                }
                else if ((board.boardArray[square + diagonalOneSquare * (i + 1)] & colour) == colour)  //  If own piece, break
                {
                    break;
                }
            }
            count++;
        }

        return moves;
    }

    public static List<int> RookMoves (Board board, int square)
    {
        int colour = board.GetActiveColourAsInt();
        List<int> moves = new List<int>();
        (int col, int row) = BoardHelperFunctions.GetIndexFromSquare(square);
        int opponentColour = colour == Pieces.White ? Pieces.Black : Pieces.White;

        //  Determine maximum number of moves along row and column
        int northCount = row;
        int southCount = 7 - row;
        int eastCount = 7 - col;
        int westCount = col;
        int[] directionLengths = new int[4] { northCount, southCount, eastCount, westCount };

        //  Add moves until end of board unless a piece can be captured, or own piece blocks column/row.
        int count = 0;
        int directionOneSquare;
        foreach (int directionLength in directionLengths)
        {
            if (count == 0)
            {
                directionOneSquare = OneSquare.north;
            }
            else if (count == 1)
            {
                directionOneSquare = OneSquare.south;
            }
            else if (count == 2)
            {
                directionOneSquare = OneSquare.east;
            }
            else
            {
                directionOneSquare = OneSquare.west;
            }

            for (int i = 0; i < directionLength; i++)
            {
                if (board.boardArray[square + directionOneSquare * (i + 1)] == Pieces.None)  //  If empty space, add move
                {
                    moves.Add(square + directionOneSquare * (i + 1));
                }
                else if ((board.boardArray[square + directionOneSquare * (i + 1)] & opponentColour) == opponentColour) //  If opponent piece, add move and break
                {
                    moves.Add(square + directionOneSquare * (i + 1));
                    break;
                }
                else if ((board.boardArray[square + directionOneSquare * (i + 1)] & colour) == colour)  //  If own piece, break
                {
                    break;
                }
            }
            count++;
        }

        return moves;
    }

    public static List<int> QueenMoves (Board board, int square)
    {
        List<int> moves = new List<int>();

        //  Add Bishop and Rook moves
        moves.AddRange(RookMoves(board, square));
        moves.AddRange(BishopMoves(board, square));

        return moves;
    }
}

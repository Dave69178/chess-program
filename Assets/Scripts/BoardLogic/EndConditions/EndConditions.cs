using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class EndConditions
{
    //  Kings and Knight
    static readonly List<int> kingsAndWKnight = new List<int>() { (Pieces.King | Pieces.White), (Pieces.Knight | Pieces.White), (Pieces.King | Pieces.Black) };
    static readonly List<int> kingsAndBKnight = new List<int>() { (Pieces.King | Pieces.White), (Pieces.King | Pieces.Black), (Pieces.Knight | Pieces.Black) };
    static readonly List<int> kingsAndWBKnights = new List<int>() { (Pieces.King | Pieces.White), (Pieces.King | Pieces.Black), (Pieces.Knight | Pieces.White), (Pieces.Knight | Pieces.Black) };

    //  Kings and Bishop
    static readonly List<int> kingsAndWBishop = new List<int>() { (Pieces.King | Pieces.White), (Pieces.Bishop | Pieces.White), (Pieces.King | Pieces.Black) };
    static readonly List<int> kingsAndBBishop = new List<int>() { (Pieces.King | Pieces.White), (Pieces.King | Pieces.Black), (Pieces.Bishop | Pieces.Black) };
    static readonly List<int> kingsAndWBBishops = new List<int>() { (Pieces.King | Pieces.White), (Pieces.King | Pieces.Black), (Pieces.Knight | Pieces.White), (Pieces.Knight | Pieces.Black) };

    public static bool IsInsufficientMaterial (Board board)
    {
        List<int> pieceSquares = LocateAllPieces.GetAllPieceSquares(board);
        List<int> pieces = new List<int>();

        foreach (int square in pieceSquares)
        {
            pieces.Add(board.boardArray[square]);
        }

        pieces = pieces.OrderBy(x => x).ToList();

        //  If only Kings left on board
        if (pieces.Count == 2)
        {
            return true;
        }

        if (pieces.Count == 3)
        {
            if (pieces == kingsAndWKnight || pieces == kingsAndBKnight || pieces == kingsAndWBishop || pieces == kingsAndBBishop)
            {
                return true;
            }
        }

        if (pieces.Count == 4)
        {
            if (pieces == kingsAndWBKnights)
            {
                return true;
            }
            if (pieces == kingsAndWBBishops)
            {
                //  If both light/dark square bishops
                if (pieceSquares.IndexOf((Pieces.White | Pieces.Bishop)) % 2 == pieceSquares.IndexOf((Pieces.Black | Pieces.Bishop)) % 2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsThreefoldRepetition (Board board)
    {
        int repetitionCount = 1;
        string currentPosition = board.gameHistory[board.gameHistory.Count - 1];
        for (int i = board.gameHistory.Count - 5; i >= board.repetitionLimiter; i -= 2)
        {
            string pastPosition = board.gameHistory[i];
            if (currentPosition.Substring(0, currentPosition.IndexOf(" ", currentPosition.IndexOf(" ") + 1) + 1) == pastPosition.Substring(0, pastPosition.IndexOf(" ", pastPosition.IndexOf(" ") + 1) + 1))
            {
                repetitionCount++;
            }
            if (repetitionCount == 3)
            {
                return true;
            }
        }
        return false;
        /*
        List<string> reversibleHistory = board.gameHistory.GetRange(board.repetitionLimiter, board.gameHistory.Count - 1);
        var reversibleHistoryEdit = reversibleHistory.Select(position => position = position.Substring(0, position.IndexOf(" ", position.IndexOf(" ") + 1) + 1));  //  Remove counts to compare positions

        if (reversibleHistoryEdit.GroupBy(x => x).Where(g => g.Count() == 3).Select(x => x.Key).ToList().Count > 0)  //  If position appears thrice, return true
        {
            return true;
        }
        else
        {
            return false;
        }
        */
    }

    public static bool IsCheckMate (Board board)
    {
        int colour = board.GetActiveColourAsInt();
        int[] boardArray = board.boardArray;
        int kingSquare = Array.IndexOf(boardArray, (Pieces.King | colour));

        //  If King in check
        if (CheckDetection.IsSquareAttacked(board, kingSquare))
        {
            //  Check for existence of legal moves
            foreach (int square in ActivePieces.GetActivePieceSquares(board))
            {
                if (LegalMoves.GetLegalMovesForSquare(board, square).Count == 0)
                {
                    continue;
                }
                return false;
            }
            return true;
        }
        return false;
    }

    public static bool IsStaleMate (Board board)
    {
        int colour = board.GetActiveColourAsInt();
        int[] boardArray = board.boardArray;
        int kingSquare = Array.IndexOf(boardArray, (Pieces.King | colour));

        //  If king is not in check
        if (!CheckDetection.IsSquareAttacked(board, kingSquare))
        {
            //  Check for existence of legal moves
            foreach (int square in ActivePieces.GetActivePieceSquares(board))
            {
                if (LegalMoves.GetLegalMovesForSquare(board, square).Count == 0)
                {
                    continue;
                }
                return false;
            }
            return true;
        }
        return false;
    }

    public static bool IsFiftyMoveRule (Board board)
    {
        if (board.halfMoveClock >= 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

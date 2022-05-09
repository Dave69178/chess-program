using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomAI
{
    public static (int, int, int) GenerateMove (Board board)
    {
        int colour = board.GetActiveColourAsInt();
        List<int> activePieces = ActivePieces.GetActivePieceSquares(board);
        List<int> activePiecesWithMoves = new List<int>();
        foreach (int piece in activePieces)
        {
            if (LegalMoves.GetLegalMovesForSquare(board, piece).Count > 0)
            {
                activePiecesWithMoves.Add(piece);
            }
        }
        int pieceIndex = Random.Range(0, activePiecesWithMoves.Count);
        int pieceSquare = activePiecesWithMoves[pieceIndex];
        List<int> legalMovesForPiece = LegalMoves.GetLegalMovesForSquare(board, pieceSquare);
        int moveIndex = Random.Range(0, legalMovesForPiece.Count);
        int moveSquare = legalMovesForPiece[moveIndex];

        int promotionPiece = Pieces.None;
        if ((board.boardArray[pieceSquare] & 7) == Pieces.Pawn && (moveSquare / 8 == 7 || moveSquare / 8 == 0))
        {
            int[] promotionPieces = { colour | Pieces.Queen, colour | Pieces.Rook, colour | Pieces.Bishop, colour | Pieces.Knight };
            int promotionPieceIndex = Random.Range(0, 4);
            promotionPiece = promotionPieces[promotionPieceIndex];
        }


        return (pieceSquare, moveSquare, promotionPiece);
    }
}

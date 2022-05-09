using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPieces : MonoBehaviour
{
    static GameObject wPawn;
    static GameObject bPawn;
    static GameObject wKnight;
    static GameObject bKnight;
    static GameObject wBishop;
    static GameObject bBishop;
    static GameObject wRook;
    static GameObject bRook;
    static GameObject wQueen;
    static GameObject bQueen;
    static GameObject wKing;
    static GameObject bKing;
    static GameObject holder;
    static GameObject pieceStore;

    private void Awake()
    {
        wPawn = Resources.Load<GameObject>("Prefabs/Pieces/w_pawn_1x_ns");
        bPawn = Resources.Load<GameObject>("Prefabs/Pieces/b_pawn_1x_ns");
        wKnight = Resources.Load<GameObject>("Prefabs/Pieces/w_knight_1x_ns");
        bKnight = Resources.Load<GameObject>("Prefabs/Pieces/b_knight_1x_ns");
        wBishop = Resources.Load<GameObject>("Prefabs/Pieces/w_bishop_1x_ns");
        bBishop = Resources.Load<GameObject>("Prefabs/Pieces/b_bishop_1x_ns");
        wRook = Resources.Load<GameObject>("Prefabs/Pieces/w_rook_1x_ns");
        bRook = Resources.Load<GameObject>("Prefabs/Pieces/b_rook_1x_ns");
        wQueen = Resources.Load<GameObject>("Prefabs/Pieces/w_queen_1x_ns");
        bQueen = Resources.Load<GameObject>("Prefabs/Pieces/b_queen_1x_ns");
        wKing = Resources.Load<GameObject>("Prefabs/Pieces/w_king_1x_ns");
        bKing = Resources.Load<GameObject>("Prefabs/Pieces/b_king_1x_ns");
        pieceStore = new GameObject("PieceStore");
    }

    public static void SetPieceGameObjectFromInt(int piece)
    {
        switch (piece)
        {
            case Pieces.Pawn | Pieces.White:
                holder = wPawn;
                break;

            case Pieces.Pawn | Pieces.Black:
                holder = bPawn;
                break;

            case Pieces.Knight | Pieces.White:
                holder = wKnight;
                break;

            case Pieces.Knight | Pieces.Black:
                holder = bKnight;
                break;

            case Pieces.Bishop | Pieces.White:
                holder = wBishop;
                break;

            case Pieces.Bishop | Pieces.Black:
                holder = bBishop;
                break;

            case Pieces.Rook | Pieces.White:
                holder = wRook;
                break;

            case Pieces.Rook | Pieces.Black:
                holder = bRook;
                break;

            case Pieces.Queen | Pieces.White:
                holder = wQueen;
                break;

            case Pieces.Queen | Pieces.Black:
                holder = bQueen;
                break;

            case Pieces.King | Pieces.White:
                holder = wKing;
                break;

            case Pieces.King | Pieces.Black:
                holder = bKing;
                break;
        }
    }

    public static void ClearBoard()
    {
        foreach (Transform child in pieceStore.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void DrawPiecesFromArray (int[] boardArray)
    {
        for (int i = 0; i < 64; i++)
        {
            if (boardArray[i] == 0)
            {
                continue;
            }
            else
            {
                (float x, float y) = PositionHelper.ArrayPositionToWorldPosition(i);
                SetPieceGameObjectFromInt(boardArray[i]);
                GameObject pieceSprite = Instantiate(holder, new Vector3(x, y, 0), Quaternion.identity);
                pieceSprite.transform.parent = pieceStore.transform;
            }
        }
    }
}

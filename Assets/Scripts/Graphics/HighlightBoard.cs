using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBoard : MonoBehaviour
{
    static SpriteRenderer squareRenderer;

    static Color OriginalSquareColor (int i)
    {
        if ((i / 8) % 2 == 0)
        {
            if (i % 2 == 0)
            {
                return Color.white;
            }
            else
            {
                return Color.black;
            }    
        }
        else
        {
            if (i % 2 == 0)
            {
                return Color.black;
            }
            else
            {
                return Color.white;
            }
        }
    }

    public static void HighlightActivePieceSquare (int i)
    {
        squareRenderer = GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>();
        squareRenderer.color = Color.red;
    }

    public static void UnhighlightActivePieceSquare(int i)
    {
        squareRenderer = GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>();
        squareRenderer.color = OriginalSquareColor(i);
    }

    public static void HighlightLegalMoves(Board board, int i)
    {
        foreach (int move in LegalMoves.GetLegalMovesForSquare(board, i))
        {
            squareRenderer = GameObject.Find(move.ToString()).GetComponent<SpriteRenderer>();
            squareRenderer.color = Color.blue;
        }
    }

    public static void UnHighlightLegalMoves(Board board, int i)
    {
        foreach (int move in LegalMoves.GetLegalMovesForSquare(board, i))
        {
            squareRenderer = GameObject.Find(move.ToString()).GetComponent<SpriteRenderer>();
            squareRenderer.color = OriginalSquareColor(move);
        }
    }
}

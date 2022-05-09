using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlgebraicNotationConversion
{
    public static int AlgebraicSquareToInt (string algebraicSquare)
    {
        return ((7 - (algebraicSquare[1] - '1')) * 8 + (algebraicSquare[0] - '0'));
    }

    public static Dictionary<string, int> AlgebraicFileToInt = new Dictionary<string, int>()
    {
        { "a", 0 },
        { "b", 1 },
        { "c", 2 },
        { "d", 3 },
        { "e", 4 },
        { "f", 5 },
        { "g", 6 },
        { "h", 7 },
    };

    public static Dictionary<int, string> IntFileToAlgebraicFile = new Dictionary<int, string>()
    {
        { 0, "a" },
        { 1, "b" },
        { 2, "c" },
        { 3, "d" },
        { 4, "e" },
        { 5, "f" },
        { 6, "g" },
        { 7, "h" },
    };

    public static string IntToAlgebraicSquare (int square)
    {
        string algebraicSquare = "";
        
        algebraicSquare += IntFileToAlgebraicFile[square % 8];
        algebraicSquare += 8 - square / 8;

        return algebraicSquare;
    }

    public static Dictionary<char, int> AlgebraicToBoardPieces = new Dictionary<char, int>()
    {
        { 'p', (Pieces.Pawn | Pieces.Black) },
        { 'k', (Pieces.King | Pieces.Black) },
        { 'n', (Pieces.Knight | Pieces.Black) },
        { 'b', (Pieces.Bishop | Pieces.Black) },
        { 'r', (Pieces.Rook | Pieces.Black) },
        { 'q', (Pieces.Queen | Pieces.Black) },
        { 'P', (Pieces.Pawn | Pieces.White) },
        { 'K', (Pieces.King | Pieces.White) },
        { 'N', (Pieces.Knight | Pieces.White) },
        { 'B', (Pieces.Bishop | Pieces.White) },
        { 'R', (Pieces.Rook | Pieces.White) },
        { 'Q', (Pieces.Queen | Pieces.White) },
    };

    public static Dictionary<int, char> BoardPiecesToAlgrebaric = new Dictionary<int, char>()
    {
        { (Pieces.Pawn | Pieces.Black), 'p' },
        { (Pieces.King | Pieces.Black), 'k' },
        { (Pieces.Knight | Pieces.Black), 'n' },
        { (Pieces.Bishop | Pieces.Black), 'b' },
        { (Pieces.Rook | Pieces.Black), 'r' },
        { (Pieces.Queen | Pieces.Black), 'q' },
        { (Pieces.Pawn | Pieces.White), 'P' },
        { (Pieces.King | Pieces.White), 'K' },
        { (Pieces.Knight | Pieces.White), 'N' },
        { (Pieces.Bishop | Pieces.White), 'B' },
        { (Pieces.Rook | Pieces.White), 'R' },
        { (Pieces.Queen | Pieces.White), 'Q' },
    };
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OneSquare
{
    public static int north = -8;
    public static int south = 8;
    public static int west = -1;
    public static int east = 1;
    public static int northWest = -9;
    public static int northEast = -7;
    public static int southWest = 7;
    public static int southEast = 9;

    public static int[] surroundingSquares = new int[] { north, northEast, east, southEast, south, southWest, west, northWest };

    public static List<int> GetSurroundingSquares (int square)
    {
        List<int> surroundingSquaresOnBoard = new List<int>() { north, northEast, east, southEast, south, southWest, west, northWest };
        (int column, int row) = BoardHelperFunctions.GetIndexFromSquare(square);
        if (column == 0)
        {
            surroundingSquaresOnBoard.Remove(northWest);
            surroundingSquaresOnBoard.Remove(west);
            surroundingSquaresOnBoard.Remove(southWest);
        }
        else if (column == 7)
        {
            surroundingSquaresOnBoard.Remove(northEast);
            surroundingSquaresOnBoard.Remove(east);
            surroundingSquaresOnBoard.Remove(southEast);
        }

        if (row == 0)
        {
            surroundingSquaresOnBoard.Remove(northWest);
            surroundingSquaresOnBoard.Remove(north);
            surroundingSquaresOnBoard.Remove(northEast);
        }
        else if (row == 7)
        {
            surroundingSquaresOnBoard.Remove(southWest);
            surroundingSquaresOnBoard.Remove(south);
            surroundingSquaresOnBoard.Remove(southEast);
        }

        return surroundingSquaresOnBoard;
    }
}

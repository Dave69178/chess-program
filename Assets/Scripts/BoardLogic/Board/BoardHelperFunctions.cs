using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardHelperFunctions
{
    public static (int column, int row) GetIndexFromSquare (int square)
    {
        return (square % 8, square / 8);
    }
}

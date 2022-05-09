using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHistory
{
    public static void AddPositionFenToHistory (Board board)
    {
        board.gameHistory.Add(FenString.BoardToFenString(board));
    }
}

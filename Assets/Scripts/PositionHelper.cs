using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionHelper
{
    public static (float, float) ArrayPositionToWorldPosition (int i)
    {
        return ((i % 8) + 0.5f, 7.5f - (i / 8));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB
{
    public Vector2 min, max;
}

public static class Utilities {

    public static bool TestOverlap(AABB a, AABB b)
    {
        if ((b.min.x <= a.max.x && b.max.x >= a.max.x) || (b.max.x >= a.min.x && b.min.x <= a.min.x))
        {
            if ((b.min.y <= a.max.y && b.max.y >= a.max.y) || (b.max.y >= a.min.y && b.min.y <= a.min.y))
                return true;
        }
        return false;
    }
}

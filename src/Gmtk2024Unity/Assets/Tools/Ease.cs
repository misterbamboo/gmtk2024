using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Ease
{
    public static float Spike(float t)
    {
        if (t <= .5f)
            return EaseIn(t / .5f);

        return EaseIn(Flip(t) / .5f);
    }

    public static float Flip(float t)
    {
        return 1 - t;
    }

    public static float EaseIn(float t)
    {
        return t * t;
    }

    public static float OutElastic(float t)
    {
        float p = 0.3f;
        return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - p / 4f) * (2f * Mathf.PI) / p) + 1f;
    }

    public static float EaseOverBack(float t)
    {
        return t * Mathf.Sin(t * Mathf.PI) + t;
    }
}
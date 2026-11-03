using UnityEngine;

public static class Ease
{
    public static float SmoothStep(float t)
    {
        t = Mathf.Clamp01(t);

        return t * t * (3f - 2f * t);
    }

    public static float OutQuad(float t)
    {
        t = Mathf.Clamp01(t);

        return t * (2f - t);
    }
}
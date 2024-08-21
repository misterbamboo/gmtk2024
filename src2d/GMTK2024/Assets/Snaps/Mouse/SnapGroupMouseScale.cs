using UnityEngine;

public class SnapGroupMouseScale
{
    private readonly Rigidbody2D rb2d;
    private float scale;

    public SnapGroupMouseScale(Rigidbody2D rb2d)
    {
        this.rb2d = rb2d;
        scale = 1;
    }

    internal void ApplyScale(float scaleDelta)
    {
        scale += scaleDelta * 0.1f;
        scale = Mathf.Clamp(scale, 0.5f, 1.5f);
        rb2d.transform.localScale = new Vector3(scale, scale, 1);
    }
}
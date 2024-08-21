using UnityEngine;

public class SnapGroupMouseRotation
{
    private readonly Rigidbody2D rb2d;

    public SnapGroupMouseRotation(Rigidbody2D rb2d)
    {
        this.rb2d = rb2d;
    }

    internal void ApplyRotation(float angle)
    {
        rb2d.rotation += angle;
    }
}
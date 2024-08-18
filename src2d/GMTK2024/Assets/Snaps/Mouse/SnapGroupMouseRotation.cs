using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SnapGroupMouseRotation
{
    private readonly Rigidbody2D rb2d;
    private float rotation;
    private int index;

    public SnapGroupMouseRotation(Rigidbody2D rb2d)
    {
        this.rb2d = rb2d;
    }

    internal void KeepInitialRotation()
    {
        rotation = rb2d.rotation;
        index = (int)(rb2d.rotation / 15f);
    }

    internal void ApplyRotationWithMouseDelta()
    {
        index += (int)Input.mouseScrollDelta.y;
        var newRotation = index * 15f;
        rb2d.rotation = newRotation;
    }
}
using System;
using UnityEngine;

public class SnapGroupDrag
{
    public bool DragRequested { get; private set; }

    public bool IsDragging { get; private set; }
    public Vector2 DraggingOffset { get; private set; }

    private readonly Transform transform;
    private readonly Camera mainCamera;

    public SnapGroupDrag(Transform transform, Camera mainCamera)
    {
        this.transform = transform;
        this.mainCamera = mainCamera;
    }

    internal void CheckRequestDrag(bool isHover)
    {
        if (!isHover)
        {
            DragRequested = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            DragRequested = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            DragRequested = false;
        }
    }

    internal void StartDrag(Vector2 hoverOffset)
    {
        if (!IsDragging)
        {
            DraggingOffset = hoverOffset;
            IsDragging = true;
        }

        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var newPos = (Vector2)mousePos + DraggingOffset;

        transform.position = newPos;
    }

    internal void EndDrag()
    {
        IsDragging = false;
    }
}

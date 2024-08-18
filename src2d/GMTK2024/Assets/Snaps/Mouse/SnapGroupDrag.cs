using System;
using UnityEngine;

public class SnapGroupDrag
{
    public bool DragRequested { get; private set; }

    public bool IsDragging { get; private set; }
    public Vector2 DraggingOffset { get; private set; }
    public bool StartDragging { get; private set; }

    private readonly Transform transform;
    private readonly Camera mainCamera;

    public SnapGroupDrag(Transform transform, Camera mainCamera)
    {
        this.transform = transform;
        this.mainCamera = mainCamera;
    }

    internal void CheckRequestDrag(bool isHover, bool inBuildMode)
    {
        if (isHover && Input.GetMouseButtonDown(0))
        {
            DragRequested = true;
        }
        if (!inBuildMode || Input.GetMouseButtonUp(0))
        {
            DragRequested = false;
        }
    }

    internal void StartOrContinueDrag(Vector2 hoverOffset)
    {
        StartDragging = false;
        if (!IsDragging)
        {
            DraggingOffset = hoverOffset;
            IsDragging = true;
            StartDragging = true;
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

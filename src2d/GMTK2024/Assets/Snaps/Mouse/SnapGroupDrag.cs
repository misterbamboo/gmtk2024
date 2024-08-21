using System;
using UnityEngine;

public class SnapGroupDrag
{
    public bool DragRequested { get; private set; }
    public bool RotateRequested { get; private set; }
    public bool ScaleRequested { get; private set; }
    public float ScaleFactor { get; private set; }

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
        RotateRequested = false;
        ScaleRequested = false;
        ScaleFactor = 0;

        var leftClickDown = Input.GetMouseButtonDown(0);
        if (isHover && leftClickDown)
        {
            DragRequested = true;
        }

        var rightClickDown = Input.GetMouseButtonDown(1);
        if (isHover && rightClickDown)
        {
            RotateRequested = rightClickDown;
        }

        var scaleFactor = (int)Mathf.Clamp(Input.mouseScrollDelta.y, -1f, 1f);
        if (scaleFactor != 0)
        {
            ScaleRequested = true;
            ScaleFactor = scaleFactor;
        }

        var leftClickUp = Input.GetMouseButtonUp(0);
        if (!inBuildMode || leftClickUp)
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

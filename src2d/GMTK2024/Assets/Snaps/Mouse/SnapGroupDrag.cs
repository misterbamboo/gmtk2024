using UnityEngine;

public class SnapGroupDrag
{
    public bool DragRequested { get; private set; }
    public bool ResizeMode { get; private set; }
    private Vector2 startResizeMousePos;
    private float startResizeScale;


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
        var leftClickDown = Input.GetMouseButtonDown(0);
        var rightClickDown = Input.GetMouseButtonDown(1);
        var clickDown = (leftClickDown || rightClickDown);
        if (isHover && clickDown)
        {
            DragRequested = true;
            ResizeMode = rightClickDown;
            if (ResizeMode)
            {
                var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                startResizeMousePos = (Vector2)mousePos + DraggingOffset;
                startResizeScale = transform.localScale.x;
            }
        }

        var leftClickUp = Input.GetMouseButtonUp(0);
        var rightClickUp = Input.GetMouseButtonUp(1);
        var clickUp = (leftClickUp || rightClickUp);

        if (!inBuildMode || clickUp)
        {
            DragRequested = false;
            ResizeMode = false;
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
        if (ResizeMode)
        {
            var xDiff = (newPos - startResizeMousePos).x;
            var targetScale = startResizeScale + xDiff;
            var scale = Mathf.Clamp(targetScale, 0.5f, 1.5f);
            transform.localScale = new Vector3(scale, scale, 1);
        }
        // MoveMode
        else
        {
            transform.position = newPos;
        }
    }

    internal void EndDrag()
    {
        IsDragging = false;
    }
}

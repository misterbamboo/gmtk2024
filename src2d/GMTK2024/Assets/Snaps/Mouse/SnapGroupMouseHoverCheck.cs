using UnityEngine;

public class SnapGroupMouseHoverCheck
{
    private readonly Transform transform;
    private readonly Camera mainCamera;
    private readonly PolygonCollider2D colliderZone;

    public bool IsHover => isHover;
    private bool isHover;

    public Vector2 HoverOffset => hoverOffset;
    private Vector2 hoverOffset;

    public SnapGroupMouseHoverCheck(Transform transform, Camera mainCamera, PolygonCollider2D colliderZone)
    {
        this.transform = transform;
        this.mainCamera = mainCamera;
        this.colliderZone = colliderZone;
    }

    internal void CheckHover(bool inBuildMode)
    {
        var previousHover = isHover;

        if (inBuildMode)
        {
            CheckMouseHover();
        }

        if (previousHover != isHover)
        {
            GameEvents.Raise(GameEvents.OnDraggableHover, isHover);
        }
    }

    private void CheckMouseHover()
    {
        isHover = false;
        hoverOffset = Vector2.zero;
        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition), 100);
        if (rayHit.collider != null && rayHit.collider == colliderZone)
        {
            isHover = true;
            var mousePos = (Vector2)rayHit.point;
            hoverOffset = (Vector2)transform.position - mousePos;
        }
    }
}

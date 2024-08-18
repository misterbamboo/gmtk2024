using UnityEngine;

public class SnapGroup : MonoBehaviour
{
    private SnapGroupMouseHoverCheck mouseHoverCheck;
    private SnapGroupDrag snapGroupDrag;

    private void Start()
    {
        var mainCamera = Camera.main;
        var colliderZone = GetComponent<PolygonCollider2D>();
        mouseHoverCheck = new SnapGroupMouseHoverCheck(transform, mainCamera, colliderZone);
        snapGroupDrag = new SnapGroupDrag(transform, mainCamera);
    }

    private void Update()
    {
        mouseHoverCheck.CheckHover();
        snapGroupDrag.CheckRequestDrag(mouseHoverCheck.IsHover);
        if (snapGroupDrag.DragRequested)
        {
            snapGroupDrag.StartDrag(mouseHoverCheck.HoverOffset);
        }
        else
        {
            snapGroupDrag.EndDrag();
        }

        print($"{mouseHoverCheck.IsHover} {mouseHoverCheck.HoverOffset} {snapGroupDrag.DragRequested}");
    }
}

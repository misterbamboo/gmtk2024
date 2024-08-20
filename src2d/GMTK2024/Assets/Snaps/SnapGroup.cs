using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapGroup : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private SnapGroupMouseHoverCheck mouseHoverCheck;
    private SnapGroupDrag snapGroupDrag;
    private SnapGroupMouseRotation snapGroupMouseRotation;
    private List<Snap> innerSnaps;
    private IGameManager gameManager;

    private bool initialized = false;
    private void Init()
    {
        if (initialized) return;
        var mainCamera = Camera.main;
        var colliderZone = GetComponent<PolygonCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        innerSnaps = GetComponentsInChildren<Snap>().ToList();

        mouseHoverCheck = new SnapGroupMouseHoverCheck(transform, mainCamera, colliderZone);
        snapGroupDrag = new SnapGroupDrag(transform, mainCamera);
        snapGroupMouseRotation = new SnapGroupMouseRotation(rb2d);
        gameManager = GameManager.Instance;
        initialized = true;
    }

    private void Update()
    {
        Init();

        mouseHoverCheck.CheckHover(gameManager.BuildActive);
        snapGroupDrag.CheckRequestDrag(mouseHoverCheck.IsHover, gameManager.BuildActive);

        if (snapGroupDrag.DragRequested)
        {
            snapGroupDrag.StartOrContinueDrag(mouseHoverCheck.HoverOffset);
            if (snapGroupDrag.StartDragging)
            {
                snapGroupMouseRotation.KeepInitialRotation();
                foreach (var snap in innerSnaps)
                {
                    snap.Unsnap();
                }
            }
        }

        if (snapGroupDrag.IsDragging)
        {
            LetSnapBeAttracted();
            snapGroupMouseRotation.ApplyRotationWithMouseDelta();
        }

        if (snapGroupDrag.IsDragging && !snapGroupDrag.DragRequested)
        {
            TrySnapNearest();
            snapGroupDrag.EndDrag();
        }
        if (!gameManager.BuildActive)
        {
            snapGroupDrag.EndDrag();
        }

        ChangeRigidBodyBehavior();
    }

    private void LetSnapBeAttracted()
    {
        foreach (var snap in innerSnaps)
        {
            if (snap.IsAttracted())
            {
                var attractedPos = transform.position + snap.AttractedDirection();
                var t = Ease.EaseIn(snap.AttractedT * 10f);
                t = Mathf.Clamp01(t) * 0.75f;

                var targetPos = Vector2.Lerp(transform.position, attractedPos, t);
                if (!float.IsNaN(targetPos.x) && !float.IsNaN(targetPos.y))
                {
                    transform.position = targetPos;
                }
            }
        }
    }

    private void TrySnapNearest()
    {
        Snap mostAttractedSnap = null;
        float lowestDistance = float.MaxValue;
        foreach (var innerSnap in innerSnaps)
        {
            if (innerSnap.IsAttracted())
            {
                var distance = innerSnap.AttractedDirection().magnitude;
                if (distance < lowestDistance)
                {
                    mostAttractedSnap = innerSnap;
                    lowestDistance = distance;
                }
            }
        }

        if (mostAttractedSnap != null)
        {
            // get closed snap
            mostAttractedSnap.SnapToClosestAttractedSnap();

            //var attractedPos = transform.position + mostAttractedSnap.AttractedDirection();
            //var t = Ease.EaseIn(snap.AttractedT * 10f);
            //t = Mathf.Clamp01(t) * 0.75f;
            //transform.position = Vector2.Lerp(transform.position, attractedPos, t);
        }
    }

    private void ChangeRigidBodyBehavior()
    {
        if (rb2d == null) return;

        var anyInnerSnapIsSnapped = innerSnaps.Any(s => s != null && s.SnapTo != null);
        if (gameManager.BuildActive || snapGroupDrag.IsDragging || anyInnerSnapIsSnapped)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    internal void MoveTo(Vector2 movement)
    {
        var pos = Vector2.Lerp(rb2d.position, rb2d.position + movement, 0.99f);
        rb2d.MovePosition(pos);
    }
}

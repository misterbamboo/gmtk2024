using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public float AttractedT => attractedTime;

    public Snap SnapTo { get; private set; }

    private List<Snap> attractedSnaps = new List<Snap>();
    private float attractedTime;
    private SnapGroup snapGroup;

    public Color DefaultColor => defaultColor;
    private Color defaultColor;

    public Vector3 AttractedDirection()
    {
        var direction = Vector3.zero;
        CleanDisposedSnaps();
        foreach (var attractedSnap in attractedSnaps)
        {
            direction += attractedSnap.transform.position - transform.position;
        }
        return direction / attractedSnaps.Count;
    }

    public bool IsAttracted()
    {
        return attractedSnaps.Any();
    }

    public void SnapToClosestAttractedSnap()
    {
        var closestSnap = attractedSnaps.OrderBy(snap => (snap.transform.position - transform.position).magnitude).FirstOrDefault();
        if (closestSnap != null)
        {
            SnapTo = closestSnap;
            closestSnap.SnapTo = this;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherSnap = collision.GetComponent<Snap>();
        attractedSnaps.Add(otherSnap);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var otherSnap = collision.GetComponent<Snap>();
        if (attractedSnaps.Contains(otherSnap))
        {
            attractedSnaps.Remove(otherSnap);
        }
    }

    private void Start()
    {
        snapGroup = GetComponentInParent<SnapGroup>();
        defaultColor = GetComponentInChildren<SpriteRenderer>().color;
    }

    private void Update()
    {
        CleanDisposedSnaps();
        IncrementAttractedTime();
        MoveInDirectionOfSnappedTo();
    }

    private void CleanDisposedSnaps()
    {
        attractedSnaps.RemoveAll(snap => snap == null);
    }

    private void IncrementAttractedTime()
    {
        if (attractedSnaps.Any())
        {
            attractedTime += Time.deltaTime;
            if (attractedTime > 1f)
            {
                attractedTime = 1f;
            }
        }
        else
        {
            attractedTime = 0f;
        }
    }

    private void MoveInDirectionOfSnappedTo()
    {
        if (SnapTo != null)
        {
            var diff = SnapTo.transform.position - transform.position;
            var movement = diff;

            // Some snaps are static and are not part of a SnapGroup
            if (snapGroup != null)
            {
                snapGroup.MoveTo(movement);
            }
        }
    }

    internal void Unsnap()
    {
        if (SnapTo != null)
        {
            SnapTo.SnapTo = null;
        }

        SnapTo = null;
    }
}

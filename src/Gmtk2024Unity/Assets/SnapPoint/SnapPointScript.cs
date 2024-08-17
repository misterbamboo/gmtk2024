using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapPointScript : MonoBehaviour
{
    private static bool visible;
    private static List<SnapPointScript> snapPoints = new List<SnapPointScript>();
    private static void Register(SnapPointScript snapPointScript)
    {
        snapPoints.Add(snapPointScript);
        snapPointScript.snapPointUI.gameObject.SetActive(false);
    }

    internal static void ShowAll()
    {
        visible = true;
        RefreshUIVisible();
    }

    internal static void HideAll()
    {
        visible = false;
        RefreshUIVisible();
    }

    private static void RefreshUIVisible()
    {
        foreach (var snapPoint in snapPoints)
        {
            if (snapPoint != null)
                snapPoint.snapPointUI.gameObject.SetActive(visible);
        }
    }

    private Camera _mainCamera;
    private Vector3 currentDraggableOffset;
    private DraggableScript currentDraggable;

    [SerializeField] float distance = 0.3f;
    [SerializeField] Transform snapPointUI;

    private float maxDistance;


    private void Awake()
    {
        SnapPointScript.Register(this);
        _mainCamera = Camera.main;
    }

    void Start()
    {
        var coll2D = GetComponent<CircleCollider2D>();
        coll2D.radius = distance;
    }

    void Update()
    {
        if (currentDraggable)
        {
            Vector3 mousePos = GetMouseWorldPos();
            var otherDraggableCenter = mousePos;
            var otherSnapCenter = otherDraggableCenter + currentDraggableOffset;
            var thisSnapCenter = transform.position;

            var distance = Vector3.Distance(thisSnapCenter, otherSnapCenter);
            if (distance > maxDistance)
            {
                print("distance: " + distance + " > maxDistance: " + maxDistance);
                TriggerUnsnap(currentDraggable);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name + " enters in " + this.name);
        var otherSnap = collision.GetComponent<SnapPointScript>();
        if (otherSnap)
        {
            print("snap enter");
            maxDistance = otherSnap.distance + distance;

            var otherDraggable = otherSnap.GetComponentInParent<DraggableScript>();
            if (otherDraggable)
            {
                print("draggable enter");
                TriggerSnap(otherDraggable, otherSnap);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentDraggable = null;
    }

    private void TriggerSnap(DraggableScript otherDraggable, SnapPointScript otherSnap)
    {
        //print("snap " + draggable.name + " to " + this.name);
        print("otherSnapPos: " + otherSnap.transform.position + " otherDraggablePos: " + otherDraggable.transform.position);
        currentDraggableOffset = otherSnap.transform.position - otherDraggable.transform.position;
        currentDraggable = otherDraggable;
        otherDraggable.SnapTo(this, currentDraggableOffset);
    }

    private void TriggerUnsnap(DraggableScript draggable)
    {
        //print("unsnap" + draggable.name + " to " + this.name);
        draggable.Unsnap();
    }

    private Vector3 GetMouseWorldPos()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -_mainCamera.transform.position.z;
        var mousePos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0;
        return mousePos;
    }


}

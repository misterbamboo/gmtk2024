using UnityEngine;

public class SnapPointScript : MonoBehaviour
{
    private Camera _mainCamera;
    private DraggableScript currentDraggable;

    [SerializeField] float distance = 0.25f;

    private float maxDistance;

    private void Awake()
    {
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
            var mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -_mainCamera.transform.position.z;
            var mousePos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
            mousePos.z = 0;

            var distance = Vector3.Distance(transform.position, mousePos);
            if (distance > maxDistance)
            {
                //print("distance: " + distance + " > maxDistance: " + maxDistance);
                TriggerUnsnap(currentDraggable);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.name + " enters in " + this.name);
        var snap = collision.GetComponent<SnapPointScript>();
        if (snap)
        {
            //print("snap enter");
            maxDistance = snap.distance + distance;

            var draggable = snap.GetComponentInParent<DraggableScript>();
            if (draggable)
            {
                //print("draggable enter");
                TriggerSnap(draggable);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentDraggable = null;
    }

    private void TriggerSnap(DraggableScript draggable)
    {
        //print("snap " + draggable.name + " to " + this.name);
        draggable.SnapTo(this);
        currentDraggable = draggable;
    }

    private void TriggerUnsnap(DraggableScript draggable)
    {
        //print("unsnap" + draggable.name + " to " + this.name);
        draggable.Unsnap();
    }
}

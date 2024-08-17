using UnityEngine;

public class DraggableScript : MonoBehaviour
{
    [SerializeField] float snapSpeed = 20f;

    private Camera _mainCamera;
    private Transform _target;
    private bool _drag;

    private Transform _snapTarget;
    private float _snapTime;
    private Vector3 _initialLerpPosition;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Start()
    {

    }

    void Update()
    {
        LookForTarget();
        UpdateDragStatus();

        if (_snapTarget)
        {
            FollowSnap();
        }
        else
        {
            FollowDrag();
        }
    }



    private void LookForTarget()
    {
        if (_drag) return;

        int layerMask;
        layerMask = 1 << LayerMask.NameToLayer("Draggable");

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Input.mousePosition), 100, layerMask);
        if (rayHit.collider && rayHit.collider.GetComponent<DraggableScript>())
        {
            //print("in");
            _target = rayHit.collider.transform;
        }
        else
        {
            //print("out");
            _target = null;
        }
    }

    private void UpdateDragStatus()
    {
        if (Input.GetMouseButtonDown(0) && _target != null)
        {
            _drag = true;
            //print("start drag");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _target = null;
            _drag = false;
            //print("end drag");
        }
    }

    private void FollowSnap()
    {
        _snapTime += Time.deltaTime;
        if (_snapTime > 1)
        {
            _snapTime = 1;
        }

        var t = Mathf.Clamp(_snapTime * snapSpeed, 0, 1);

        // make animation bounce
        t = Ease.EaseOverBack(t);

        var snapAnimPos = Vector3.Lerp(_initialLerpPosition, _snapTarget.position, t); // 0.1f is the time to snap
        transform.position = snapAnimPos;
    }

    private void FollowDrag()
    {
        if (!_drag) return;

        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -_mainCamera.transform.position.z;
        var mousePos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0;
        _target.position = mousePos;
        //print(Input.mousePosition.ToString() + " " + mousePos.ToString() + " " + pos.ToString() + " " + _target.position.ToString());
    }

    internal void SnapTo(SnapPointScript snapPointScript)
    {
        _snapTarget = snapPointScript.transform;

        _snapTime = 0;
        _initialLerpPosition = transform.position;
    }

    internal void Unsnap()
    {
        _snapTarget = null;
    }
}

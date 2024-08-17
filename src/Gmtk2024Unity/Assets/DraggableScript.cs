using UnityEngine;

public class DraggableScript : MonoBehaviour
{
    [SerializeField] float snapSpeed = 20f;

    private Camera _mainCamera;
    private Transform _target;
    private bool _drag;

    private Transform _snapTarget;
    private Vector3 _snapTargetOffset;
    private float _snapTime;
    private Vector3 _initialLerpPosition;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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

        var previousTarget = _target;

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

        if (previousTarget != _target)
        {
            GameEvents.Raise(GameEvents.OnDraggableHover, _target != null);
        }
    }

    private void UpdateDragStatus()
    {
        if (Input.GetMouseButtonDown(0) && _target != null)
        {
            _drag = true;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            rb2d.freezeRotation = true;
            SnapPointScript.ShowAll();
            //print("start drag");
        }
        else if (_drag && Input.GetMouseButtonUp(0))
        {
            _target = null;
            _drag = false;
            SnapPointScript.HideAll();
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;

            if (_snapTarget == null)
            {
                rb2d.freezeRotation = false;
                rb2d.isKinematic = false;
            }
            else
            {
                rb2d.freezeRotation = true;
                rb2d.isKinematic = true;
            }
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

        var targetPos = _snapTarget.position - _snapTargetOffset;
        var snapAnimPos = Vector3.Lerp(_initialLerpPosition, targetPos, t); // 0.1f is the time to snap
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

    internal void SnapTo(SnapPointScript snapPointScript, Vector3 snapOffset)
    {
        _snapTarget = snapPointScript.transform;
        _snapTargetOffset = snapOffset;

        //print("_snapTargetOffset: " + _snapTargetOffset);

        _snapTime = 0;
        _initialLerpPosition = transform.position;
    }

    internal void Unsnap()
    {
        _snapTarget = null;
    }
}

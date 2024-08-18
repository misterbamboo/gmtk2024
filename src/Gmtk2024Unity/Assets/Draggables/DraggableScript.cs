using UnityEngine;

public class DraggableScript : MonoBehaviour
{
    public bool IsDragged => _drag;
    private bool _drag;

    [SerializeField] bool activateLogs = false;
    [SerializeField] float snapSpeed = 20f;

    private Camera _mainCamera;
    private Transform _target;
    private Transform _hover;

    private SnapPointScript _snap;
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
        CheckIfHover();
        UpdateDragStatus();

        if (_snap != null)
        {
            FollowSnap();
        }
        else if (_drag)
        {
            FollowDrag();
        }
    }

    private void CheckIfHover()
    {
        //if (_drag) return;

        int layerMask;
        layerMask = 1 << LayerMask.NameToLayer("Draggable");

        var previousHover = _hover;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Input.mousePosition), 100, layerMask);
        if (rayHit.collider && rayHit.collider.GetComponent<DraggableScript>())
        {
            Log("in");
            _hover = rayHit.collider.transform;
        }
        else
        {
            Log("out");
            _hover = null;
        }

        if (previousHover != _hover)
        {
            GameEvents.Raise(GameEvents.OnDraggableHover, _hover != null);
        }
    }

    private void UpdateDragStatus()
    {
        if (Input.GetMouseButtonDown(0) && _hover != null)
        {
            _target = _hover;
            _drag = true;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            rb2d.freezeRotation = true;
            SnapPointScript.ShowAll();

            if (_snap != null)
            {
                _snap.AssignDraggable(this);
            }

            Log("start drag");
        }
        else if (_drag && Input.GetMouseButtonUp(0))
        {
            _target = null;
            _drag = false;
            SnapPointScript.HideAll();
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;

            if (_snap == null)
            {
                rb2d.freezeRotation = false;
                rb2d.isKinematic = false;
            }
            else
            {
                rb2d.freezeRotation = true;
                rb2d.isKinematic = true;
            }
            Log("end drag");
        }
    }

    private void FollowSnap()
    {
        Log($"followSnap - [this: {this.name}, _snapTarget: {_snap.name}] : {_snapTime}s");
        _snapTime += Time.deltaTime;
        if (_snapTime > 1)
        {
            _snapTime = 1;
        }

        var t = Mathf.Clamp(_snapTime * snapSpeed, 0, 1);

        // make animation bounce
        t = Ease.EaseOverBack(t);

        var targetPos = _snap.transform.position - _snapTargetOffset;
        var snapAnimPos = Vector3.Lerp(_initialLerpPosition, targetPos, t); // 0.1f is the time to snap
        transform.position = snapAnimPos;
    }

    private void FollowDrag()
    {
        if (!_drag) return;

        Log($"followDrag - [this: {this.name}]");
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -_mainCamera.transform.position.z;
        var mousePos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0;
        _target.position = mousePos;
        Log($"mousePosition: {Input.mousePosition} mouseScreenPos:{mouseScreenPos} mousePos:{mousePos} _targetPos:{_target.position}");
    }

    internal void SnapTo(SnapPointScript snapPointScript, Vector3 snapOffset)
    {
        _snap = snapPointScript;
        _snapTargetOffset = snapOffset;

        Log("_snapTargetOffset: " + _snapTargetOffset);

        _snapTime = 0;
        _initialLerpPosition = transform.position;
    }

    internal void Unsnap()
    {
        _snap = null;
    }

    private void Log(string log)
    {
        if (activateLogs)
        {
            print(log);
        }
    }
}

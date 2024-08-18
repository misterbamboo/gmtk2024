using UnityEngine;
using UnityEngine.PlayerLoop;

public class CusorManager : MonoBehaviour
{
    [SerializeField] Texture2D cursorDefaultTexture;
    [SerializeField] Texture2D cursorDefaultClickTexture;
    [SerializeField] Texture2D selectCursorDefaultTexture;
    [SerializeField] Texture2D selectCursorDefaultClickTexture;

    Texture2D _cursorDefaultTexture;
    Texture2D _cursorDefaultClickTexture;
    Texture2D _selectCursorDefaultTexture;
    Texture2D _selectCursorDefaultClickTexture;

    private Vector2 cursorHotspot;

    private bool CursorHoverDraggable { get; set; }
    private bool CursorMouseDown { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.SubscribeTo<bool>(GameEvents.OnDraggableHover, OnDraggableHover);
        _cursorDefaultTexture = cursorDefaultTexture;
        _cursorDefaultClickTexture = cursorDefaultClickTexture;
        _selectCursorDefaultTexture = selectCursorDefaultTexture;
        _selectCursorDefaultClickTexture = selectCursorDefaultClickTexture;

        cursorHotspot = new Vector2(_cursorDefaultTexture.width / 2, _cursorDefaultTexture.height * 55f / 512f);

        Cursor.SetCursor(_cursorDefaultTexture, cursorHotspot, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CursorMouseDown = true;
            UpdateCursor();
        }
        if (Input.GetMouseButtonUp(0))
        {
            CursorMouseDown = false;
            UpdateCursor();
        }
    }

    private void OnDraggableHover(bool hover)
    {
        CursorHoverDraggable = hover;
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        print($"CursorHoverDraggable: {CursorHoverDraggable} CursorMouseDown: {CursorMouseDown}");

        var mouseUpTexture = CursorHoverDraggable ? _selectCursorDefaultTexture : _cursorDefaultTexture;
        var mouseDownTexture = CursorHoverDraggable ? _selectCursorDefaultClickTexture : _cursorDefaultClickTexture;

        var mouseTexture = CursorMouseDown ? mouseDownTexture : mouseUpTexture;

        Cursor.SetCursor(mouseTexture, cursorHotspot, CursorMode.ForceSoftware);
    }
}

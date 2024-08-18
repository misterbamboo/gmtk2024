using UnityEngine;

public class ParallaxBackgroundFollow : MonoBehaviour
{
    [SerializeField] Transform background11;
    [SerializeField] Transform background12;
    [SerializeField] Transform background21;
    [SerializeField] Transform background22;

    float parallaxSpeed = 2;
    private float _backgroundWidth;
    private float _backgroundHeight;
    private Transform _cameraFollow;

    void Start()
    {
        var sr = background11.GetComponent<SpriteRenderer>();
        _backgroundWidth = sr.sprite.bounds.size.x;
        _backgroundHeight = sr.sprite.bounds.size.y;
        _cameraFollow = CameraFollow.Instance.transform;
    }

    void Update()
    {
        // X parralax
        var playerPosRelativeX = _cameraFollow.position.x / parallaxSpeed;
        var backgroundXPage = (int)(playerPosRelativeX / _backgroundWidth);
        var pageX = backgroundXPage * _backgroundWidth;

        // Y parralax
        var playerPosRelativeY = _cameraFollow.position.y / parallaxSpeed;
        var backgroundYPage = (int)(playerPosRelativeY / _backgroundHeight);
        var pageY = backgroundYPage * _backgroundHeight - 1;

        var x = pageX + _cameraFollow.position.x * (1 / parallaxSpeed);
        var y = pageY + _cameraFollow.position.y * (1 / parallaxSpeed);

        // top left
        background11.position = new Vector3(x, y, background11.position.z);

        // top right
        var bg2Pos = background11.position;
        bg2Pos.x += _backgroundWidth;
        background12.position = bg2Pos;

        // bottom left
        var bg3Pos = background11.position;
        bg3Pos.y -= _backgroundHeight;
        background21.position = bg3Pos;

        // bottom right
        var bg4Pos = background12.position;
        bg4Pos.y -= _backgroundHeight;
        background22.position = bg4Pos;
    }
}

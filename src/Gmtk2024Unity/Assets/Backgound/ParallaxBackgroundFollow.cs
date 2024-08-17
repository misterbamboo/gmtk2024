using UnityEngine;

public class ParallaxBackgroundFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Transform background1;
    [SerializeField] Transform background2;

    float parallaxSpeed = 2;

    private float _backgroundWidth;

    void Start()
    {
        _backgroundWidth = background1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }

    void Update()
    {
        var playerPosRelative = player.position.x / parallaxSpeed;
        var backgroundPage = (int)(playerPosRelative / _backgroundWidth);

        var pageX = backgroundPage * _backgroundWidth;

        background1.position = new Vector3(pageX + player.position.x * (1 / parallaxSpeed), background1.position.y, background1.position.z);

        var bg2Pos = background1.position;
        bg2Pos.x += _backgroundWidth;
        background2.position = bg2Pos;
    }
}

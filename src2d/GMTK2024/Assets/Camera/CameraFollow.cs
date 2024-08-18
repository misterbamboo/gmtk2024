using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow _instance;
    public static CameraFollow Instance => _instance;

    [SerializeField] Transform target;
    [SerializeField] float yOffset = 3.5f;

    private Vector2 bottomLeft, topRight;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            // Camera follows the player
            var pos = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);

            // Clamp camera position
            pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
            pos.y = Mathf.Clamp(pos.y, bottomLeft.y, topRight.y);

            transform.position = pos;
        }
    }

    public void UpdateClamp(Vector2 bottomLeft, Vector2 topRight)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }
}

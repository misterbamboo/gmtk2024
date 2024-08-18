using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow _instance;
    public static CameraFollow Instance => _instance;

    [SerializeField] Transform target;
    [SerializeField] float yOffset = 3.5f;

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
            transform.position = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
        }
    }
}

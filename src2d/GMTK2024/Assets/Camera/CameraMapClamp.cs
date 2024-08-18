using UnityEngine;

public class CameraMapClamp : MonoBehaviour
{
    [SerializeField] Vector2 bottomLeft;
    [SerializeField] Vector2 topRight;

    private CameraFollow camFollow;

    private void Start()
    {
        camFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        camFollow.UpdateClamp(bottomLeft, topRight);
    }
}

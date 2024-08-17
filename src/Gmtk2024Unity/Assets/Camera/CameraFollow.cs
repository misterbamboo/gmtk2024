using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Camera follows the player
        transform.position = new Vector3(target.position.x, 0, transform.position.z);
    }
}

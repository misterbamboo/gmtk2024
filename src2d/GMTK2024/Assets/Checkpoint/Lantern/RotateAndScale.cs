using UnityEngine;

public class RotateAndScale : MonoBehaviour
{
    [SerializeField] Transform light;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float scaleSpeed = 1;
    float time = 0;
    private IGameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (gameManager.BuildActive) return;

        time += Time.deltaTime;
        if (time > float.MaxValue - 1024f)
        {
            time = 0;
        }

        var angles = light.rotation.eulerAngles;
        angles.z = time * 360f * rotateSpeed;
        light.rotation = Quaternion.Euler(angles);

        var scale = ScaleEase(time * scaleSpeed);
        light.localScale = new Vector3(scale, scale, 1);
    }

    private float ScaleEase(float t)
    {
        return Mathf.Sin(6 * t) * Mathf.Cos(2 * t) * 0.2f + 0.8f;
    }
}

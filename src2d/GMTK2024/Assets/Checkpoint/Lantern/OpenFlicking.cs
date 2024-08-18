using UnityEngine;

public class OpenFlicking : MonoBehaviour
{
    [SerializeField] SpriteRenderer light;
    [SerializeField] float flickSpeed = 3;

    [SerializeField]
    [Range(0, 3)]
    float flickTime = 0;

    bool initialized = false;
    private IGameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnDrawGizmos()
    {
        UpdateLight();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (initialized) return;
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            initialized = true;
        }
    }

    void Update()
    {
        if (!initialized) return;

        if (!gameManager.BuildActive)
        {
            flickTime += Time.deltaTime * flickSpeed;
            UpdateLight();
        }
    }

    private void UpdateLight()
    {
        if (light)
        {
            var intensity = FlickingEase(flickTime);
            intensity = Mathf.Clamp01(intensity);

            var transparentWhite = new Color(1, 1, 1, intensity);
            light.color = transparentWhite;
        }
    }

    // https://www.desmos.com/calculator/eeul1ruhqh
    private float FlickingEase(float t)
    {
        float part1 = Mathf.Sin(t * 21f);
        float part2 = Mathf.Cos(t * 1.6f);
        float part3 = t / 2f;
        return part1 * part2 + part3;
    }
}

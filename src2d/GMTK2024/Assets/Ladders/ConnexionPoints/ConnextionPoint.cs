using System.Linq;
using UnityEngine;

public class ConnextionPoint : MonoBehaviour
{
    private static readonly Color transparent = new Color(1, 1, 1, 0);

    [SerializeField] private ConnextionPointOptions options;
    private SpriteRenderer spriteRenderer;
    private IGameManager gameManager;
    private bool previousBuildActive;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (gameManager.BuildActive != previousBuildActive)
        {
            ChangeVisibility(gameManager.BuildActive);
        }
        previousBuildActive = gameManager.BuildActive;
    }

    internal void ChangeVisibility(bool buildActive)
    {
        spriteRenderer.color = buildActive ? Color.white : transparent;

        if (buildActive && options && options.sprites != null && options.sprites.Any())
        {
            spriteRenderer.sprite = options.sprites[Random.Range(0, options.sprites.Length)];
        }
    }
}

using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] GameObject root;

    private IGameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (root.activeInHierarchy != gameManager.BuildMenu)
        {
            root.SetActive(gameManager.BuildMenu);
        }

        if (gameManager.BuildMenu)
        {
        }
    }
}

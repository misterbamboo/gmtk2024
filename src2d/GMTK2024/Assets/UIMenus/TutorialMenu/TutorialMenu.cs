using UnityEngine;

public class TutorialMenu : MonoBehaviour
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
        if (root.activeInHierarchy != gameManager.TutorialMenu)
        {
            root.SetActive(gameManager.TutorialMenu);
        }

        if (gameManager.TutorialMenu && Input.GetMouseButtonDown(0))
        {
            gameManager.StartGame();
        }
    }
}

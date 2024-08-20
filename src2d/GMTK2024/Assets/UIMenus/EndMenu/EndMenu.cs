using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
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
        if (root.activeInHierarchy != gameManager.EndMenu)
        {
            root.SetActive(gameManager.EndMenu);
        }

        if (gameManager.EndMenu && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}

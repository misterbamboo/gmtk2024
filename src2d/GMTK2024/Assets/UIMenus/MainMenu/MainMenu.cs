using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject root;
    private IGameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (root.activeInHierarchy != gameManager.MainMenu)
        {
            root.SetActive(gameManager.MainMenu);
        }

        if (gameManager.MainMenu)
        {
            MainMenuUpdate();
        }
    }

    private void MainMenuUpdate()
    {
    }

    public void Play_Clicked()
    {
        gameManager.Tutorial();
    }

    public void Quit_Clicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

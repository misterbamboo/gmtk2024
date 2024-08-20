using UnityEngine;

public interface IGameManager
{
    // MENUS
    bool MainMenu { get; }
    bool TutorialMenu { get; }
    bool BuildMenu { get; }
    bool EndMenu { get; }
    // END MENUS

    bool BuildActive { get; }

    void Respawn(Transform player);
    void RegisterCheckpoint(Vector3 checkpointPos);
    void StartGame();
    void Tutorial();
    void TriggerEndGame();
}

public class GameManager : MonoBehaviour, IGameManager
{
    // MENUS
    [SerializeField] bool _mainMenu;
    public bool MainMenu => _mainMenu;

    [SerializeField] bool _tutorialMenu;
    public bool TutorialMenu => _tutorialMenu;

    [SerializeField] bool _buildMenu;
    public bool BuildMenu => _buildMenu;

    [SerializeField] bool _endMenu;
    public bool EndMenu => _endMenu;
    // END MENUS

    public static IGameManager Instance => _instance;
    private static GameManager _instance;

    public bool BuildActive => buildActive;


    private bool buildActive;
    private Vector3 currentCheckpointPos;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            ChangeMainMenu();
        }
    }

    private void Start()
    {
        GameEvents.SubscribeTo<bool>(GameEvents.OnBuildModeActiveChanged, OnBuildModeActiveChanged);
    }

    private void OnBuildModeActiveChanged(bool buildActive)
    {
        this.buildActive = buildActive;
    }

    public void Respawn(Transform player)
    {
        player.position = currentCheckpointPos;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void RegisterCheckpoint(Vector3 checkpointPos)
    {
        if (currentCheckpointPos.x < checkpointPos.x)
        {
            currentCheckpointPos = checkpointPos;
        }
    }

    public void ChangeMainMenu()
    {
        _mainMenu = true;
        _tutorialMenu = false;
        _buildMenu = false;
        _endMenu = false;
    }

    public void Tutorial()
    {
        _mainMenu = false;
        _tutorialMenu = true;
        _buildMenu = false;
        _endMenu = false;
    }

    public void StartGame()
    {
        _mainMenu = false;
        _tutorialMenu = false;
        _buildMenu = true;
        _endMenu = false;
    }

    public void TriggerEndGame()
    {
        _mainMenu = false;
        _tutorialMenu = false;
        _buildMenu = false;
        _endMenu = true;
    }
}

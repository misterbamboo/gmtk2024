using UnityEngine;
using UnityEngine.SceneManagement;

public interface IGameManager
{
    bool BuildActive { get; }
    void Respawn(Transform player);
    void RegisterCheckpoint(Vector3 checkpointPos);
}

public class GameManager : MonoBehaviour, IGameManager
{
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
}

using UnityEngine;

public interface IGameManager
{
    bool BuildActive { get; }
}

public class GameManager : MonoBehaviour, IGameManager
{
    public static IGameManager Instance => _instance;
    private static GameManager _instance;

    public bool BuildActive => buildActive;
    private bool buildActive;

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
        print("buildActive: " + buildActive);
        this.buildActive = buildActive;
    }
}

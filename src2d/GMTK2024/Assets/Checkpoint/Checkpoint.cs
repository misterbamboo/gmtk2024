using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Vector3 checkpointPos;
    private IGameManager gameManager;
    private bool triggered = false;

    void Start()
    {
        var pos = transform.position;
        pos.z = 0;
        checkpointPos = pos;
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        var player = collision.GetComponent<PlayerMovement>();
        if (player)
        {
            if (!triggered)
            {
                gameManager.RegisterCheckpoint(checkpointPos);
                triggered = true;
            }
        }
    }
}

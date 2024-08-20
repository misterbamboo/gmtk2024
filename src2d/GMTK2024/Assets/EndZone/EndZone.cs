using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] bool animDone;

    [SerializeField] Transform animStartPos;
    private Rigidbody2D playerTransform;
    private float playerPosX;
    private float playerTimeToStartPos;

    private Animator animator;
    private bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        // get animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            playerTimeToStartPos += Time.deltaTime * 4f; // 0.25 secs
            playerTimeToStartPos = Mathf.Clamp01(playerTimeToStartPos);

            if (!isPlaying)
            {
                var initialPos = new Vector3(playerPosX, playerTransform.position.y, playerTransform.transform.position.z);
                var pos = Vector3.Lerp(initialPos, animStartPos.position, playerTimeToStartPos);
                playerTransform.position = pos;

                if (playerTimeToStartPos >= 1)
                {
                    animator.SetBool("Play", true);
                }
            }

            

            // If anim playing, actif should be false. And If notplaying , actif should be true.
            if (isPlaying == playerTransform.gameObject.activeInHierarchy)
            {
                playerTransform.gameObject.SetActive(!isPlaying);
            }
        }

        isPlaying = animator.GetBool("Play");

        if (animDone)
        {
            GameManager.Instance.TriggerEndGame();
            playerTransform = null;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hit: " + collision.name);
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            print("enter");
            playerTransform = player.GetComponent<Rigidbody2D>();
            playerPosX = player.transform.position.x;
            playerTimeToStartPos = 0;
            isPlaying = false;
        }
    }
}

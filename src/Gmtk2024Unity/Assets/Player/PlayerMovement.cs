using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] SpriteRenderer animatedSprite;

    [SerializeField] SpriteRenderer leftWallSprite;
    [SerializeField] SpriteRenderer rightWallSprite;

    [SerializeField] float jumpForce = 5f;
    [SerializeField] float releaseDownForce = 1f;
    [SerializeField] float lateralForce = 5f;

    [SerializeField] Transform slopeCheckCenter;
    [SerializeField] float slopeCheckLength = 0.5f;
    [SerializeField] float maxSlopeAngle = 50f;

    private Rigidbody2D rb2d;
    private PlayerFloorDetection playerFloorDetection;
    private Animator animator;
    private Vector2 velocity = Vector2.zero;

    private float timeInactive;

    public bool IsSpacebarPressed { get; private set; } = false;
    public bool SlopeBlockJump { get; private set; }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerFloorDetection = GetComponentsInChildren<PlayerFloorDetection>().FirstOrDefault();
        animator = GetComponentsInChildren<Animator>().FirstOrDefault();

        leftWallSprite.gameObject.SetActive(false);
        rightWallSprite.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.zero;

        HorizontalMovement();
        JumpMovement();

        var currentVelocity = rb2d.velocity;
        currentVelocity.x = 0;
        var newVelocity = currentVelocity + velocity;
        rb2d.velocity = newVelocity;
    }

    private void HorizontalMovement()
    {
        SlopeBlockJump = false;
        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            if (!CheckBlockedBySlopeAngle(Vector3.right))
            {
                velocity += horizontal * Vector2.right * lateralForce;
            }
            else
            {
                SlopeBlockJump = true;
            }
            animatedSprite.flipX = false;
        }

        if (horizontal < 0)
        {
            if (!CheckBlockedBySlopeAngle(Vector3.left))
            {
                velocity += horizontal * Vector2.right * lateralForce;
            }
            else
            {
                SlopeBlockJump = true;
            }
            animatedSprite.flipX = true;
        }

        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        if (velocity.magnitude < 0.02)
        {
            timeInactive += Time.deltaTime;
        }
        else
        {
            timeInactive = 0;
        }
        animator.SetBool("Sleeping", timeInactive > 5);
    }

    private bool CheckBlockedBySlopeAngle(Vector3 checkDirection)
    {
        int fullMask = GetFullHitMask();
        var hit = Physics2D.Raycast(slopeCheckCenter.position, checkDirection, slopeCheckLength, fullMask);
        Debug.DrawLine(slopeCheckCenter.position, slopeCheckCenter.position + checkDirection * slopeCheckLength, Color.red);
        if (hit && hit.collider)
        {
            var perc = Vector2.Perpendicular(hit.normal);
            var absPerc = new Vector2(Mathf.Abs(perc.x), Mathf.Abs(perc.y));
            // get angule of the normal
            var angle = Vector2.SignedAngle(Vector2.right, absPerc);
            //print("normal: " + hit.normal + " absPerc: " + absPerc + " angle:" + angle);
            return angle > maxSlopeAngle;
        }
        return false;
    }

    private static int GetFullHitMask()
    {
        var mask1 = LayerMask.GetMask("Draggable");
        var mask2 = LayerMask.GetMask("Floor");
        var mask3 = LayerMask.GetMask("Default");
        var fullMask = mask1 | mask2 | mask3;
        return fullMask;
    }

    private void JumpMovement()
    {
        var space = Input.GetKey(KeyCode.Space);
        if (space)
        {
            if (!IsSpacebarPressed && playerFloorDetection.OnFloor)
            {
                if (!SlopeBlockJump)
                {
                    velocity += Vector2.up * jumpForce;
                }
            }
            IsSpacebarPressed = true;
        }
        else
        {
            if (!playerFloorDetection.OnFloor)
            {
                if (!SlopeBlockJump)
                {
                    velocity += Vector2.down * releaseDownForce;
                }
            }
            IsSpacebarPressed = false;
        }

        animator.SetBool("Jumping", !playerFloorDetection.OnFloor);
        
        bool jumpRaising = velocity.y > 1f;
        animator.SetBool("JumpRaising", jumpRaising);

        bool jumpFalling = rb2d.velocity.y < 0.02f; // want to check falling with actual real velocity
        animator.SetBool("JumpFalling", jumpFalling);
    }
}

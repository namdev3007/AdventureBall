using UnityEngine;

[System.Serializable]
public class StatPlayer
{
    public AnimationCurve speedPlayer;
    public float maxVelocity;
    public float scaleSpeed;
    public AnimationCurve addForcePos;
    public float maxAddForcePos;
    public float maxAngleVelo;
}

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public StatPlayer groundStart;
    public StatPlayer jumpStart;
    public float forceUpPlayer;
    public AnimationCurve velocityJumpX;
    public Vector2 checkGroundLeft;
    public float sizeCheckMove;
    public LayerMask layerJump;

    private StatPlayer currentStart;
    private Vector2 checkGroundRight;
    private float sizeCheckLeftRight;

    private int moveInput = 0;
    private bool jumpInput = false;
    private bool isGround = false;
    private bool isWall = false;
    private float timeJump = 0;
    private float lastVelocityGroundY = 0;
    private RaycastHit2D hit;

    void Awake()
    {
        checkGroundRight = new Vector2(-checkGroundLeft.x, checkGroundLeft.y);
        sizeCheckLeftRight = checkGroundLeft.magnitude;
    }

    public void Reset()
    {
        rb.linearVelocity = Vector2.zero;
        isGround = false;
        timeJump = 0;
        moveInput = 0;
        jumpInput = false;
        rb.simulated = true;
        transform.rotation = Quaternion.identity;
        currentStart = jumpStart;
    }

    void Update()
    {
        isGround = CheckGrounded();
        currentStart = isGround ? groundStart : jumpStart;

        if (!isGround)
        {
            timeJump += Time.deltaTime;
        }
        else
        {
            timeJump = 0;
        }

        lastVelocityGroundY = rb.linearVelocity.y;

        if (moveInput != 0)
        {
            PlayerMove(moveInput);
        }

        if (jumpInput)
        {
            Jump();
        }
    }

    private bool CheckGrounded()
    {
        Vector2 position = transform.position;
        hit = Physics2D.Raycast(position, checkGroundLeft, sizeCheckLeftRight, layerJump);
        if (hit.collider != null) return true;

        hit = Physics2D.Raycast(position, checkGroundRight, sizeCheckLeftRight, layerJump);
        return hit.collider != null;
    }

    void Jump()
    {
        if (isGround)
        {
            float jumpForceX = (moveInput == 0)
                ? rb.linearVelocity.x * velocityJumpX.Evaluate(Mathf.Clamp01(Mathf.Abs(rb.linearVelocity.x) / groundStart.maxVelocity))
                : rb.linearVelocity.x;

            rb.linearVelocity = new Vector2(jumpForceX, forceUpPlayer);
        }
    }

    public void PlayerMove(int input)
    {
        isWall = !isGround && Physics2D.Raycast(transform.position, Vector2.right * input, sizeCheckMove, layerJump).collider != null;

        if ((-currentStart.maxVelocity < rb.linearVelocity.x && rb.linearVelocity.x < currentStart.maxVelocity) || (rb.linearVelocity.x * input < 0))
        {
            if (!(rb.linearVelocity.y > 0.1f && isWall))
            {
                float forceX = input * currentStart.scaleSpeed * currentStart.maxVelocity *
                               currentStart.speedPlayer.Evaluate(Mathf.Clamp01(rb.linearVelocity.x * input / currentStart.maxVelocity));
                float forceYPos = currentStart.maxAddForcePos *
                                  currentStart.addForcePos.Evaluate(Mathf.Clamp01(Mathf.Abs(rb.linearVelocity.x) / currentStart.maxVelocity));

                rb.AddForceAtPosition(new Vector2(forceX, 0), transform.position + Vector3.up * forceYPos);
            }
        }
    }

    public void UpdateMovementRight() => moveInput = 1;
    public void UpdateMovementLeft() => moveInput = -1;
    public void UpdateMovementUp() => moveInput = 0;
    public void UpdateJumpping() => jumpInput = true;
    public void NotJumpping() => jumpInput = false;

    private void OnDrawGizmosSelected()
    {
        Vector2 position = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, position + checkGroundLeft);
        Gizmos.DrawLine(position, position + Vector2.right * sizeCheckMove);
    }
}

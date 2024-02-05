using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Jump")]
    public float jumpSpeed = 15;
    public float normalGravity = 5.3f;
    public float heldJumpGravity = 0.47f;
    public float fallGravity = 1.64f;

    [Header("Move")]
    public float currentSpeedX;

    public float runSpeedX = 30;

    [Header("Ground Detection")]
    public LayerMask groundLayers;
    public Vector3 rGroundoffset = new Vector3(0.5f, -1.02f, 0f);
    private Vector3 lGroundoffset => new Vector3(-rGroundoffset.x, rGroundoffset.y, rGroundoffset.z);

    [Header("Instant Move")]
    public Vector3 touchPosition = new Vector3(33, -100, -1);
    public Vector3 targetPosition = new Vector3(192f, 4f, -1f);
    public float disThreshold = 4.0f;

    private Rigidbody2D rigid;

    private float inputValx = 0;

    private bool isJumpHeld;
    private bool isOnGround;
    private bool isJumpRelased;
    private bool isFlag;



    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        isJumpRelased = true;
        isFlag = false;
    }



    // Update is called once per frame
    void Update()
    {
        isJumpHeld = Input.GetKey(KeyCode.Space);
        inputValx = Input.GetAxis("Horizontal");
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumpRelased = true;
        }

        isOnGround = Physics2D.OverlapPoint((Vector2)(transform.position + rGroundoffset), groundLayers) || 
                     Physics2D.OverlapPoint((Vector2)(transform.position + lGroundoffset), groundLayers);

        if(Vector3.Distance(transform.position, touchPosition) < disThreshold)
        {
            transform.position = targetPosition;
        }

        if(transform.position.x >=103 && transform.position.x <= 175)
        {
            isFlag = true;
        }
        else
        {
             isFlag=false;
        }
    }

    private void FixedUpdate()
    {
        var vel = rigid.velocity;
        var velY = vel.y;
        if (isOnGround)
        {
            if (isJumpHeld && isJumpRelased)
            {
                velY = jumpSpeed;
                isJumpRelased = false;
            }
        } else
        {
            if (velY > 0)
            {
                if(isJumpHeld)
                {
                    rigid.gravityScale = normalGravity * heldJumpGravity;
                } else
                {
                    rigid.gravityScale = normalGravity;
                }
            }
        }

        vel.y = velY;
        currentSpeedX = 5;
        if (isFlag)
        {
            vel.x = 15;
        } else
        {
            vel.x = inputValx * currentSpeedX;
        }
        
        
        rigid.velocity = vel;
    }
}

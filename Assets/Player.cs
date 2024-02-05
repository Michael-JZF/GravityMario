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
    public float currentSpeedX = 0f;

    public float runSpeedX = 4.0f;

    [Header("Ground Detection")]
    public LayerMask groundLayers;
    public Vector3 rGroundoffset = new Vector3(0.5f, -1.02f, 0f);
    private Vector3 lGroundoffset => new Vector3(-rGroundoffset.x, rGroundoffset.y, rGroundoffset.z);

    private Rigidbody2D rigid;

    private float inputValx = 0;

    private bool isJumpHeld;
    private bool isOnGround;
    private bool isJumpRelased;



    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        isJumpRelased = true;
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

        currentSpeedX = runSpeedX;
        vel.x = inputValx * currentSpeedX;
        rigid.velocity = vel;
    }
}

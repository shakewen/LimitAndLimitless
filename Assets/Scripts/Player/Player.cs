using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    public Animator animator;

    //��ȡ��������
    public float moveInput;

    public bool isInteract;

    public bool isTab;


    //�Ƿ�����ƶ�
    public bool canmove = true;

    public float speed = 2f;
   
    public bool faceRight = true;

    public bool isGround = false;

    public LayerMask whatIsGround;

    public Transform groundCheck;

    public float groundCheckRadius = 0.2f;

    public float jumpForce = 13.5f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator =GetComponent<Animator>();
        
        groundCheck =transform.Find("GroundCheck");

        whatIsGround = LayerMask.GetMask("Ground");
    }

    
    void Update()
    {
      
        moveInput = InputManager.GethorizontalInput();

        isInteract = InputManager.GetJInput();

        isTab = InputManager.GetTabInput();


        if(canmove&& moveInput!= 0f)
        {
            animator.SetFloat("Move", Mathf.Abs(moveInput));
        }

        //������뽻������ҳ���������ɽ�������canMoveΪfalse����ʾ���ﲻ���ƶ��ͽ��ж���

        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
       
        if (canmove)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            
        }
        else
        {
            rb.velocity = Vector2.zero;
        }


        MoveToFlip();
    }


    void MoveToFlip()
    {
        if (canmove)
        {
            if ((!faceRight && moveInput > 0f) || (faceRight && moveInput < 0f))
            {
                faceRight = !faceRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }
}

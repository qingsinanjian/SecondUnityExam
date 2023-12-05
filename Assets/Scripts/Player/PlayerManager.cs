using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //主角属性设定：昵称、生命值、魔法值、物理攻击力、魔法攻击力、移动速度；
    public string playerName;
    public int currentHP;
    public int MaxMP = 100;
    public int physicalDamage = 1;
    public int magicDamage = 2;
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;
    public float jumpForce = 600f;
    public bool isOnGround = true;

    private Rigidbody rb;
    private Animator animator;
    private float inputH;
    private float inputV;
    private int moveScale = 1;

    private void Start()
    {
        currentHP = MaxMP;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isOnGround)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                isOnGround = true;
                animator.SetBool("IsOnGround", isOnGround);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveScale = 2;
        }
        else
        {
            moveScale = 1;
        }
        inputH = Input.GetAxis("Horizontal") * moveScale;
        inputV = Input.GetAxis("Vertical") * moveScale;
        Move();
        Rotate();
        Jump();
        CommonAttack();
        PlaySkillInput();
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        if (inputV != 0)
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * moveSpeed * inputV);
            animator.SetFloat("InputH", 0);
            animator.SetFloat("InputV", inputV);
        }
        else
        {
            if (inputH != 0)
            {
                rb.MovePosition(transform.position + transform.right * Time.deltaTime * moveSpeed * inputH);
                animator.SetFloat("InputH", inputH);
            }
            else
            {
                animator.SetFloat("InputH", 0);
                animator.SetFloat("InputV", 0);
            }
        }
    }

    /// <summary>
    /// 旋转
    /// </summary>
    private void Rotate()
    {
        if (inputH != 0 && inputV != 0)
        {
            float targetRotation = rotateSpeed * inputH;
            transform.eulerAngles = Vector3.up * Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + targetRotation, Time.deltaTime);
        }
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                isOnGround = false;
                animator.SetBool("IsOnGround", isOnGround);
            }
            rb.AddForce(Vector3.up * jumpForce);
            animator.CrossFade("Jump", 0.1f);
        }
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    private void CommonAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.CrossFade("Attack", 0.1f);
        }
    }

    private void PlaySkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.CrossFade("Skill1", 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.CrossFade("Skill2", 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.CrossFade("Skill3", 0.1f);
        }
    }
}

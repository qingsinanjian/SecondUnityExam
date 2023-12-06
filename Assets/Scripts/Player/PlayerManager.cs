using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //���������趨���ǳơ�����ֵ��ħ��ֵ������������ħ�����������ƶ��ٶȡ���Ծ�����Ƿ��ڵ��棻
    public string playerName;
    public int currentHP;
    public int MaxHP = 100;
    public int currentMP;
    public int MaxMP = 100;
    public int physicalDamage = 10;
    public int magicDamage = 20;
    [Header("Buff�����������")]
    public int buffPSDamage = 15;
    [Header("Buff���ħ��������")]
    public int buffMCDamage = 30;
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;
    public float jumpForce = 600f;
    public bool isOnGround = true;
    [Header("�Ƿ�������״̬")]
    public bool isBuffing = false;
    
    //������Ч
    public GameObject[] skillEffects;
    //��ս���ܡ�Զ�̼��ܡ��������ܵ���ȴʱ��
    public float meleeCoolingTime = 2;
    private float meleeDelay;
    public float rangedCoolingTime = 3;
    private float rangedDelay;
    public float buffCoolingTime = 15;
    private float buffDelay;
    [Header("Buff����ʱ��")]
    public float buffDuration = 5;
    private float curBuffDuration;
    [Header("Զ�̼���ħ������ֵ")]
    public int rangedMP = 20;
    [Header("Buffħ������ֵ")]
    public int buffMP = 40;
    //������Ч�ͷŵĻ�׼��
    public GameObject effectPoint;

    private Rigidbody rb;
    private Animator animator;
    private float inputH;
    private float inputV;
    private int moveScale = 1;

    public bool isClone;
    public GameObject enemyPrefab;

    public GameUIController gameUIController;

    private void Start()
    {
        currentHP = MaxHP;
        currentMP = MaxMP;
        meleeDelay = meleeCoolingTime;
        rangedDelay = rangedCoolingTime;
        buffDelay = buffCoolingTime;
        curBuffDuration = buffDuration;
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
        if (isClone)
        {
            return;
        }
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
        IsBuffingState();
        CallEnemy();
        SkillCoolDown();
    }

    /// <summary>
    /// �ƶ�
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
    /// ��ת
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
    /// ��Ծ
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
    /// ��ͨ����
    /// </summary>
    private void CommonAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.CrossFade("Attack", 0.1f);
        }
    }

    /// <summary>
    /// �����ͷ�
    /// </summary>
    private void PlaySkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(meleeDelay >= meleeCoolingTime)
            {
                animator.CrossFade("Skill1", 0.1f);
                meleeDelay = 0;
                gameUIController.SkillRelease(0, true);
            }
        }
        meleeDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(currentMP >= rangedMP)
            {
                if (rangedDelay >= rangedCoolingTime)
                {
                    animator.CrossFade("Skill2", 0.1f);
                    currentMP -= rangedMP;
                    gameUIController.MPCost(currentMP, MaxMP);
                    rangedDelay = 0;
                    gameUIController.SkillRelease(1, true);
                }
            }
            else
            {
                Debug.Log("ħ��ֵ����");
            }            
        }
        rangedDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(currentMP >= buffMP)
            {
                if (buffDelay >= buffCoolingTime)
                {
                    isBuffing = true;
                    animator.CrossFade("Skill3", 0.1f);
                    currentMP -= buffMP;
                    gameUIController.MPCost(currentMP, MaxMP);
                    buffDelay = 0;
                    gameUIController.SkillRelease(2, true);
                }
            }
            else
            {
                Debug.Log("ħ��ֵ����");
            }
        }
        buffDelay += Time.deltaTime;

        IsMPEnough();
    }

    /// <summary>
    /// ������ȴ����
    /// </summary>
    private void SkillCoolDown()
    {
        if (meleeDelay < meleeCoolingTime)
        {
            float coolDown = (meleeCoolingTime - meleeDelay) / meleeCoolingTime;
            gameUIController.SkillCD(0, coolDown);
        }
        if (rangedDelay < rangedCoolingTime)
        {
            float coolDown = (rangedCoolingTime - rangedDelay) / rangedCoolingTime;
            gameUIController.SkillCD(1, coolDown);
        }
        if (buffDelay < buffCoolingTime)
        {
            float coolDown = (buffCoolingTime - buffDelay) / buffCoolingTime;
            gameUIController.SkillCD(2, coolDown);
        }
    }

    /// <summary>
    /// �ж�ħ��ֵ�Ƿ��㹻�ͷż���
    /// </summary>
    private void IsMPEnough()
    {
        gameUIController.SetISMPEnough(1, currentMP >= rangedMP);
        gameUIController.SetISMPEnough(2, currentMP >= buffMP);
    }

    /// <summary>
    /// �Ƿ���Buff״̬
    /// </summary>
    private void IsBuffingState()
    {
        if (isBuffing)
        {
            if(curBuffDuration >= 0)
            {
                curBuffDuration -= Time.deltaTime;
            }
            else
            {
                isBuffing = false;
                curBuffDuration = buffDuration;
            }
        }
    }

    /// <summary>
    /// ���Ž�ս������Ч
    /// </summary>
    private void PlayMeleeEffect()
    {
        if (isClone) return;
        Vector3 pos = effectPoint.transform.position - new Vector3(0f, 0.1f, 0);
        GameObject go = Instantiate(skillEffects[0], pos, Quaternion.identity, effectPoint.transform);
        Weapon weapon = go.GetComponent<Weapon>();
        if(isBuffing)
        {
            weapon.damageValue = buffPSDamage;
        }
        else
        {
            weapon.damageValue = physicalDamage;
        }
    }

    /// <summary>
    /// ����Զ�̼�����Ч
    /// </summary>
    private void PlayRangedEffect()
    {
        Vector3 pos = effectPoint.transform.position + new Vector3(0f, 1f, 0);
        GameObject go = Instantiate(skillEffects[1], pos, Quaternion.LookRotation(transform.forward));
        Weapon weapon = go.GetComponent<Weapon>();
        if (isBuffing)
        {
            weapon.damageValue = buffMCDamage;
        }
        else
        {
            weapon.damageValue = magicDamage;
        }
    }

    /// <summary>
    /// ����Buff��Ч
    /// </summary>
    private void PlayBuffEffect()
    {
        Vector3 pos = transform.position + new Vector3(0f, 1f, 0);
        Instantiate(skillEffects[2], pos, Quaternion.identity, transform);
    }

    /// <summary>
    /// ������ͨ������Ч
    /// </summary>
    private void PlayCommonAttackEffect()
    {
        Vector3 pos = effectPoint.transform.position - new Vector3(0f, 0f, 0);
        GameObject go = Instantiate(skillEffects[3], pos, Quaternion.identity, effectPoint.transform);
        Weapon weapon = go.GetComponent<Weapon>();
        if (isBuffing)
        {
            weapon.damageValue = buffPSDamage - 5;
        }
        else
        {
            weapon.damageValue = physicalDamage - 5;
        }
    }

    /// <summary>
    /// �ܵ��˺�
    /// </summary>
    /// <param name="damageValue"></param>
    public void TakeDamage(int damageValue)
    {
        currentHP -= damageValue;
        animator.SetTrigger("Hit");
        if(currentHP <= 0)
        {
            animator.SetBool("Die", true);
            transform.GetComponent<CapsuleCollider>().enabled = false;
            rb.isKinematic = true;
        }
    }

    /// <summary>
    /// ��C�ٻ�����
    /// </summary>
    private void CallEnemy()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject go = Instantiate(enemyPrefab, transform.position + transform.forward * 5, Quaternion.identity);
            go.transform.LookAt(transform.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //敌人属性设定：昵称、生命值、魔法值、物理攻击力、魔法攻击力、移动速度、跳跃力、是否在地面；
    public string enemyName;
    public int currentHP;
    public int MaxHP = 100;
    public int currentMP;
    public int MaxMP = 100;
    public int physicalDamage = 15;
    public int magicDamage = 25;
    [Header("Buff后的物理攻击力")]
    public int buffPSDamage = 25;
    [Header("Buff后的魔法攻击力")]
    public int buffMCDamage = 35;
    public int physicalDef = 5;
    public int magicDef = 5;
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;
    public float jumpForce = 600f;
    public bool isOnGround = true;
    [Header("是否处于增幅状态")]
    public bool isBuffing = false;

    //技能特效
    public GameObject[] skillEffects;
    //近战技能、远程技能、增幅技能的冷却时间
    public float meleeCoolingTime = 2;
    private float meleeDelay;
    public float rangedCoolingTime = 3;
    private float rangedDelay;
    public float buffCoolingTime = 15;
    private float buffDelay;
    [Header("Buff持续时间")]
    public float buffDuration = 5;
    private float curBuffDuration;
    [Header("远程技能魔法消耗值")]
    public int rangedMP = 20;
    [Header("Buff魔法消耗值")]
    public int buffMP = 40;
    //技能特效释放的基准点
    public GameObject effectPoint;

    private Rigidbody rb;
    private Animator animator;
    private float inputH;
    private float inputV;
    private int moveScale = 1;

    public GameObject enemyPrefab;

    public EnemyUI enemyUI;

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
        enemyUI.enemyNameText.text = enemyName;
        enemyUI.SetEnemyHP(currentHP, MaxHP);
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
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    moveScale = 2;
        //}
        //else
        //{
        //    moveScale = 1;
        //}
        //inputH = Input.GetAxis("Horizontal") * moveScale;
        //inputV = Input.GetAxis("Vertical") * moveScale;
        //Move();
        //Rotate();
        //Jump();
        //CommonAttack();
        //PlaySkillInput();
        //IsBuffingState();
        //SkillCoolDown();
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
        if (Input.GetMouseButtonDown(0))
        {
            animator.CrossFade("Attack", 0.1f);
        }
    }

    /// <summary>
    /// 技能释放
    /// </summary>
    private void PlaySkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (meleeDelay >= meleeCoolingTime)
            {
                animator.CrossFade("Skill1", 0.1f);
                meleeDelay = 0;
            }
        }
        meleeDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentMP >= rangedMP)
            {
                if (rangedDelay >= rangedCoolingTime)
                {
                    animator.CrossFade("Skill2", 0.1f);
                    currentMP -= rangedMP;
                    rangedDelay = 0;
                }
            }
            else
            {
                Debug.Log("魔力值不够");
            }
        }
        rangedDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentMP >= buffMP)
            {
                if (buffDelay >= buffCoolingTime)
                {
                    isBuffing = true;
                    animator.CrossFade("Skill3", 0.1f);
                    currentMP -= buffMP;
                    buffDelay = 0;
                }
            }
            else
            {
                Debug.Log("魔力值不够");
            }
        }
        buffDelay += Time.deltaTime;

        IsMPEnough();
    }

    /// <summary>
    /// 技能冷却计算
    /// </summary>
    private void SkillCoolDown()
    {
        if (meleeDelay < meleeCoolingTime)
        {
            float coolDown = (meleeCoolingTime - meleeDelay) / meleeCoolingTime;
        }
        if (rangedDelay < rangedCoolingTime)
        {
            float coolDown = (rangedCoolingTime - rangedDelay) / rangedCoolingTime;
        }
        if (buffDelay < buffCoolingTime)
        {
            float coolDown = (buffCoolingTime - buffDelay) / buffCoolingTime;
        }
    }

    /// <summary>
    /// 判断魔法值是否足够释放技能
    /// </summary>
    private void IsMPEnough()
    {
        //gameUIController.SetISMPEnough(1, currentMP >= rangedMP);
        //gameUIController.SetISMPEnough(2, currentMP >= buffMP);
    }

    /// <summary>
    /// 是否处于Buff状态
    /// </summary>
    private void IsBuffingState()
    {
        if (isBuffing)
        {
            if (curBuffDuration >= 0)
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
    /// 播放近战技能特效
    /// </summary>
    private void PlayMeleeEffect()
    {
        Vector3 pos = effectPoint.transform.position - new Vector3(0f, 0.1f, 0);
        GameObject go = Instantiate(skillEffects[0], pos, Quaternion.identity, effectPoint.transform);
        Weapon weapon = go.GetComponent<Weapon>();
        if (isBuffing)
        {
            weapon.damageValue = buffPSDamage;
        }
        else
        {
            weapon.damageValue = physicalDamage;
        }
    }

    /// <summary>
    /// 播放远程技能特效
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
    /// 播放Buff特效
    /// </summary>
    private void PlayBuffEffect()
    {
        Vector3 pos = transform.position + new Vector3(0f, 1f, 0);
        Instantiate(skillEffects[2], pos, Quaternion.identity, transform);
    }

    /// <summary>
    /// 播放普通攻击特效
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
    /// 受到伤害
    /// </summary>
    /// <param name="attribute">0为物理攻击，1为魔法攻击</param>
    /// <param name="damageValue"></param>
    public void TakeDamage(int attribute, int damageValue)
    {
        if (attribute == 0)
        {
            if (damageValue > physicalDef)
            {
                int value = damageValue - physicalDef;
                enemyUI.ShowDamageText(value);
                currentHP -= value;
            }
        }
        else if (attribute == 1)
        {
            if (damageValue > magicDef)
            {
                int value = damageValue - magicDef;
                enemyUI.ShowDamageText(value);
                currentHP -= value;
            }
        }
        enemyUI.ShowInfo();
        enemyUI.SetEnemyHP(currentHP, MaxHP);
        animator.SetTrigger("Hit");
        if (currentHP <= 0)
        {
            GameManager.Instance.enemyManagers.Remove(this);
            animator.SetBool("Die", true);
            transform.GetComponent<CapsuleCollider>().enabled = false;
            rb.isKinematic = true;
            enemyUI.gameObject.SetActive(false);
            Destroy(gameObject, 5);
        }
    }
}

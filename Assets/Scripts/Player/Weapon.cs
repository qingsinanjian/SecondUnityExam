using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public int damageValue;
    [Header("武器属性：0物理，1魔法")]
    public int weaponAttribute = -1;
    private EnemyManager enemyManager;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            enemyManager = collision.transform.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(weaponAttribute, damageValue);
        }
    }
}

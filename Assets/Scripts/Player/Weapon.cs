using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public int damageValue;
    [Header("�������ԣ�0����1ħ��")]
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

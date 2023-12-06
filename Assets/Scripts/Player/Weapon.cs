using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damageValue;
    private PlayerManager playerManager;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            playerManager = collision.transform.GetComponent<PlayerManager>();
            playerManager.TakeDamage(damageValue);
        }
    }
}

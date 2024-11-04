using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackT : MonoBehaviour
{
    public PlayerRPG playerRPG;

    private void Awake()
    {
        // PlayerController ������Ʈ ����
        playerRPG = GetComponent<PlayerRPG>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("����");
            string bossType = other.gameObject.name;
            playerRPG.DealDamageToBoss(bossType, playerRPG.attackDamage);
        }
    }
}

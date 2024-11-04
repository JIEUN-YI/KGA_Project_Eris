using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] float destorytime;
    [SerializeField] float effectDamage;
    private bool spendDamage = false;
    private void Start()
    {
        Destroy(gameObject, destorytime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spendDamage)
        {
            // �÷��̾�� �������� �ִ� ����
            PlayerRPG playerRPG = collision.GetComponent<PlayerRPG>();
            if (playerRPG != null)
            {
                playerRPG.TakeDamage(effectDamage);
                Debug.Log($"�÷��̾�� {effectDamage} �������� �������ϴ�.");
            }
            spendDamage = true;
        }
    }
}

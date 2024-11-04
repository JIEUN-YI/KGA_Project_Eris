using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bashscript : MonoBehaviour
{
    public GameObject[] Colliders;
    [SerializeField] float bashDamage;
    private bool spendDamage = false;

    private void Start()
    {
        Destroy(gameObject, 1.7f);
        StartCoroutine(Controller());
    }

    private IEnumerator Controller()
    {
        yield return new WaitForSeconds(0.5f);      
        Colliders[1].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Colliders[0].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Colliders[2].SetActive(true);
        Colliders[1].SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spendDamage)
        {
            // �÷��̾�� �������� �ִ� ����
            PlayerRPG playerRPG = collision.GetComponent<PlayerRPG>();
            if (playerRPG != null)
            {
                playerRPG.TakeDamage(bashDamage);
                Debug.Log($"�÷��̾�� {bashDamage} �������� �������ϴ�.");
            }
            spendDamage = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallScript : MonoBehaviour
{
    // �÷��̾� ������ ����
    bool spendDamage = false;
    // �÷��̾� ������
    [SerializeField] GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        // �ִϸ��̼� ���� ������
        // �ұ���� 2�ʵ� ����
        Destroy(gameObject, 2f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!spendDamage)
            {
                // �÷��̾�� �������� �ִ� ���� 

                // �ѹ��� �������� �ֱ����� spendDamage�� ������ ����
            }
            spendDamage = true;
        }

    }
}

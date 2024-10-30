using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    // �÷��̾� ������ ����
    bool spendDamage = false;
    // �÷��̾� ������
    [SerializeField] GameObject player;
    // ���̾�� ���ǵ�
    [SerializeField] float fireBallSpeed;
    // �߻� ����
    private Vector2 direction;
    private Rigidbody2D rb;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        // ���̾�� �÷��̾� ������ ���� ���
        direction = new Vector2((player.transform.position.x - transform.position.x), 0).normalized;
        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        rb.velocity = direction * fireBallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spendDamage)
        {
            // ������ �ȹ޾Ҵٸ�
            if (!spendDamage)
            {                
                // �÷��̾�� �������� �ִ� ����
            }
            // �ѹ��� �������� �ֱ����� spendDamage�� ������ ����
            spendDamage = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // �Ҹ� �ִϸ��̼� ���� ���� (�ʿ� ��)
            Destroy(gameObject); // �ִϸ��̼� �� �Ҹ�
        }
    }
}

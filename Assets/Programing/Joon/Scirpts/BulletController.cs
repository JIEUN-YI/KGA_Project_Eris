using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2D ������Ʈ

    private void Awake()
    {
        // Rigidbody2D ������Ʈ ��������
        rb = GetComponent<Rigidbody2D>();

        // �߷� ������ ���� �ʵ��� ����
        rb.gravityScale = 0;
    }

    public void SetSpeed(Vector2 speed)
    {
        if (rb != null)
        {
            rb.velocity = speed; // Rigidbody2D�� velocity�� �����Ͽ� ������ �̵�
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); // Ʈ���ŷ� ���� �� �ı�
    }
}

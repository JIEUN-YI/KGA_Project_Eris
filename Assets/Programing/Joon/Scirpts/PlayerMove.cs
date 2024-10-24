using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed = 10;
    [SerializeField] float jumpForce = 10;
    private bool isGrounded = false; // ���� ���� ���θ� ���� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 dir = new Vector2(x, 0); // 2D ȯ�濡���� z ��� y�� ���
        Walk(dir);

        // C Ű�� ������ ��, ���� ���� ���� ���� ����
        if (Input.GetKey(KeyCode.C) && isGrounded)
        {
            Jump();
        }
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;
        isGrounded = false; // ���� �Ŀ��� ���߿� �����Ƿ� false�� ����
    }

    // �ٴڿ� ��Ҵ��� üũ�ϱ� ���� �ݸ��� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground��� �±װ� �ִ� ������Ʈ�� �浹�ϸ� ���� �ִٰ� �Ǵ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // �ٴڿ��� �������� �ٽ� false�� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

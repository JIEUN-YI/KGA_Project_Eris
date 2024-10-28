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
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        // ���̾�� �÷��̾� ������ ���� ���
        direction = new Vector2((player.transform.position.x - transform.position.x), 0).normalized;
    }

    private void Update()
    {
        transform.Translate(direction * fireBallSpeed * Time.deltaTime);
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
        // ���� �ε�ġ�� �Ҹ��Ű��
        if (collision.gameObject.tag == "Ground")
        {
            // �Ҹ� �ִϸ��̼� �ִٸ� ����

            // �ִϸ��̼��� �����Ѵٸ� �ε�ģ ��ġ�� �����ϴ��ڵ�
            // ������ �ִϸ��̼� ����ð����� ����
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAura : MonoBehaviour
{
    // �÷��̾� ������
    [SerializeField] GameObject player;
    // �˱��� ���ǵ�
    [SerializeField] float swordAuraSpeed;
    // �߻� ����
    private Vector2 direction;
    private bool flipped = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // �÷��̾� ���� ���
            direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

            // ������ ������� ���� ���
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // �˱⸦ �÷��̾ ���ϵ��� ȸ��
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    private void Update()
    {
        // �˱⸦ �÷��̾� �������� �̵�
        transform.Translate(direction * swordAuraSpeed * Time.deltaTime);
    }
}

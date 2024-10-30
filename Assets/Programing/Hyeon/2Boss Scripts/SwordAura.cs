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
    
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        HomingSwordAura();

        /*
        if (player != null)
        {
            // �÷��̾� ���� ���
            direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

            // ������ ������� ���� ���
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // �˱⸦ �÷��̾ ���ϵ��� ȸ��
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }*/
    }
    /*private void Update()
    {
        // �˱⸦ �÷��̾� �������� �̵�
        transform.Translate(direction * swordAuraSpeed * Time.deltaTime);
    }*/
    private void HomingSwordAura()
    {
        Vector2 diretion = (Vector2)player.transform.position - (Vector2)transform.position;
        direction.Normalize();

        float rotate = Vector3.Cross(diretion, transform.up).z;
        transform.Rotate(0, 0, -rotate * 400f * Time.deltaTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * 10f * Time.deltaTime);
    }
}

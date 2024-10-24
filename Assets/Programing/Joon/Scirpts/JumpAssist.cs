using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAssist : MonoBehaviour
{
    //ĳ������ �������� �ӵ��� ���߽�Ű�� ���� ����
    [SerializeField] float fallMultiplier = 4f;
    //ĳ������ ����Ű �Է��� ���� �� �߷��� ������ Ű�� �� ������ ���������� ����
    [SerializeField] float lowJumpMultiplier = 7f;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //ĳ���Ͱ� �ϰ����� ��
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //ĳ���Ͱ� ����������� ���� ��ư�� ���� ��
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.C))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}

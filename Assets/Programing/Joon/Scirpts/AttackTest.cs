using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    public bool IsBossInRange { get; private set; } = false;
    public Collider2D attackRangeCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss")) // �������� �浹�� ����
        {
            IsBossInRange = true;
            Debug.Log("������ ���� ������ ���Խ��ϴ�.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLenPlatform : MonoBehaviour
{
    // ������ �ӵ���
    [SerializeField] public float moveSpeed;

    // ���⺰�� �̵�
    // PlatformAttackPattern���� ���κ��� �޾ƿ����� �����ϱ�
    [SerializeField] PatternController patternController;

    private void Update()
    {
        if (patternController.isUpMove)
        {
            MoveUp();
        }
        else if (!patternController.isUpMove)
        {
            MoveDown();
        }
    }

    /// <summary>
    /// ������ ������ �ӵ��� ���� ��� �̵�
    /// </summary>
    public void MoveUp()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }

    public void MoveDown()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

}

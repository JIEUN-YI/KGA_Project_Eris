using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLenPlatform : MonoBehaviour
{
    // ������ �ӵ���
    [SerializeField] float moveSpeed;
    // ���⺰�� �̵�
    [SerializeField] public bool isUpMove; // true �̸� ���� �̵�
    [SerializeField] GameObject DesDelete; // �ϰ����� �� ���� ���� �浹ü
    [SerializeField] GameObject RiseDelete; // ������� �� ���� ���� �浹ü

    private void Update()
    {
        if (isUpMove)
        {
            DesDelete.SetActive(false);
            RiseDelete.SetActive(true);
            MoveUp();
        }
        else if (!isUpMove)
        {
            DesDelete.SetActive(true);
            RiseDelete.SetActive(false);
            MoveDown();
        }
    }

    /// <summary>
    /// ������ ������ �ӵ��� ���� ��� �̵�
    /// </summary>
    public void MoveUp()
    {
        transform.Translate(Vector2.up * moveSpeed*Time.deltaTime);
    }

    public void MoveDown()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

}

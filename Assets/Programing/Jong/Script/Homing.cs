using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    [SerializeField] GameObject player;


    private void Update()
    {
        HomingMissile();
    }


    private void HomingMissile()  // �ƹ� ������Ʈ�� ���̰� �÷��̾� ���� ������Ʈ�� �ҷ����� ����ź�� �� 
    {
        Vector2 direction = (Vector2)player.transform.position - (Vector2)transform.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z; // ���ؾȰ� ; �ƹ�ư ������ ���� ���ؼ� �� �������� ȸ�� 
        transform.Rotate(0, 0, -rotateAmount * 400f * Time.deltaTime); // ������ ���� �÷��̾� �������� ������ Ʋ�� (y���� �÷��̾�� ���ϰ�) �̵� 
                                                                       // ���� ������ ���߷��� �̵��ӵ�, �� ȸ���ӵ� , �Լ��� ȣ��Ƚ��?(������ ���ٰ� �ƴ� �ڷ�ƾ���� ���ʸ��� �̷������� ����)�� �����ؾ���
        transform.Translate(Vector2.up * 10f * Time.deltaTime);
    }

    private void Flying() 
    {

    }

}

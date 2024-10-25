using UnityEngine;

public class FieldController : MonoBehaviour
{
    // BossPattern�� TakeDamage(); - ������ ü�� ����
    // BossObjact�� �̵��� �� �Ʒ� Ground�� ��� �� ����
    // PlayerController�� AttackUpdate()�� DieUpdate()�� ��� �� �� ����
    // HealFlat.cs�� DeleteFlat(); - HealFlat�� �浹 �� ���� �ð� �� �����ϴ� �Լ�

    /// <summary>
    /// �浹�� �߻��� ������ ��Ÿ�� �ൿ
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DeadZone")
        {
            /* tag�� DeadZone�� ��� 
             * �÷��̾��� ���
             * DieUpdate()
             */
        }

        if (collision.gameObject.tag == "HealFlat")
        {
            /* tag�� HealFlat�� ���
             * �÷��̾ ���� ���� ī���� Ȱ��ȭ
             * Ư�� ī��Ʈ�� ������ ������Ʈ �����ϵ���
             * HealFlat.cs���� �Լ� �ۼ��� �ҷ�����
             */
            // �浹�� ������Ʈ�� �ִ� HealFlat ������Ʈ �ҷ�����

        }

    }

    /// <summary>
    /// ���������� �浹 ���ε��� ���������� �߻��ϴ� �ൿ
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        /* if ( collision.gameObject.tag == "SideWall")
         * �÷��̾ SideWall �±׿� ���������� �پ��ִ� ���� ����ؼ�
         * �÷��̾��� �������� ������ �Լ� ����
         */

    }
}

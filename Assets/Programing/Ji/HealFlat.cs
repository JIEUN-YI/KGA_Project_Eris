using System.Collections;
using UnityEngine;

public class HealFlat : MonoBehaviour
{
    // �ڷ�ƾ���� �ð� Ÿ�̹��� ����
    // �ð� ���� �� ������Ʈ ����
    [Header("State")]
    [SerializeField] float DeleteTime; // �������� �ɸ��� �ð� ����

    /// <summary>
    /// HealFlat�� �浹ü�� �浹�ϴ� ���� �Ǵ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /* tag�� HealFlat�� ���
             * �÷��̾ ���� ���� ī���� Ȱ��ȭ
             * Ư�� ī��Ʈ�� ������ ������Ʈ �����ϵ���
             * HealFlat.cs���� �Լ� �ۼ��� �ҷ�����
             */
            StartCoroutine(FlatDelete());
            // DeleteFlat(); // �����ϴ� �Լ� ����
        }
    }

    /// <summary>
    /// �ڷ�ƾ���� �ð��� ������ �� ���� ������Ʈ ����
    /// </summary>
    public void DeleteFlat()
    {
        StartCoroutine(FlatDelete()); // �ڷ�ƾ���� �ð� ����
    }

    IEnumerator FlatDelete()
    {
        yield return new WaitForSeconds(DeleteTime);
        Destroy(gameObject); // �������� �ð��� ������ HealFlat ������Ʈ ����
    }
}

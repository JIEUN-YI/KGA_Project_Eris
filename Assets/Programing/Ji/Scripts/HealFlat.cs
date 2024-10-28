using System.Collections;
using UnityEngine;

public class HealFlat : MonoBehaviour
{
    // �ڷ�ƾ���� �ð� Ÿ�̹��� ����
    // �ð� ���� �� ������Ʈ ����
    [Header("State")]
    [SerializeField] float DeleteTime; // �������� �ɸ��� �ð� ����
    [SerializeField] SpriteRenderer spriteRenderer; // ������ �̹���
    /*
        /// <summary>
        /// HealFlat�� �浹ü�� �浹�ϴ� ���� �Ǵ�
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Finsh") // ���� PlayerFoot �±� �߰� �Ͽ� ���� �ʿ�
            {
                /* tag�� PlayerFoot�� ���
                 * �÷��̾ ���� ���� ī���� Ȱ��ȭ
                 * Ư�� ī��Ʈ�� ������ ������Ʈ �����ϵ���
                 * HealFlat.cs���� �Լ� �ۼ��� �ҷ�����
                 *
                StartCoroutine(FlatDelete());
                // DeleteFlat(); // �����ϴ� �Լ� ����
            }
        }
    */

    /// <summary>
    /// HealFlat�� Trigger �浹ü�� �浹�� ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish") // ���� PlayerFoot �±� �߰� �Ͽ� ���� �ʿ�
        {
            /* tag�� PlayerFoot�� ���
             * �÷��̾ ���� ���� ī���� Ȱ��ȭ
             * Ư�� ī��Ʈ�� ������ ������Ʈ �����ϵ���
             * HealFlat.cs���� �Լ� �ۼ��� �ҷ�����
             */
            Debug.Log("�浹");
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

    /// <summary>
    /// HealFlat�� �������� �����ϴ� �ڷ���
    /// </summary>
    /// <returns></returns>
    IEnumerator FlatDelete()
    {
        float timeTerm = 0.5f; // �����̴� ������ �����ϴ� Ÿ�̸�
        while (DeleteTime > 0) // ���������� �ð��� 0���� ū ���ȿ� ������ �ݺ�
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f); // �̹����� ���� 50% ����
            yield return new WaitForSeconds(timeTerm); // �����ð� ����
            DeleteTime -= timeTerm; // timeTerm�� ������ ��ŭ DeletTime ���� 
            spriteRenderer.color = new Color(1, 1, 1, 1f); // �̹����� ���� 100% ����
            yield return new WaitForSeconds(timeTerm); // �����ð� ����
            DeleteTime -= timeTerm; // timeTerm�� ������ ��ŭ DeletTime ���� 
        }
        // DeleteTime �� 0�̵Ǹ� ������Ʈ�� ������
        Destroy(gameObject); // �������� �ð��� ������ HealFlat ������Ʈ ����
    }
}

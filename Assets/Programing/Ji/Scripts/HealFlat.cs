using System.Collections;
using UnityEngine;

public class HealFlat : MonoBehaviour
{
    // �ڷ�ƾ���� �ð� Ÿ�̹��� ����
    // �ð� ���� �� ������Ʈ ����
    [Header("State")]
    [SerializeField] float DeleteTime; // �������� �ɸ��� �ð� ����
    [SerializeField] SpriteRenderer spriteRenderer; // ������ �̹���

    /// <summary>
    /// HealFlat�� �浹ü�� �浹�� ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /* tag�� Player�� ���
             * OnCollisionEnter�� �߻��ϱ� ���� collision�� relativeVelocity(�浹������ �ӵ�)�� �����ͼ�
             * relativeVelocity.y <0�� ��쿡�� �����ϴ� �ڷ�ƾ�� ����ϵ��� ��
             * 
             * ������ ����� ������ �ڷ�ƾ�� �۵��ؾ��ϹǷ�
             */
            if (collision.relativeVelocity.y < 0)
            {
                // collision�� �浹 ������ �ӵ��� 0���� �۴ٴ� ����
                // �浹ü�� ������ �Ʒ��� �������� �ִٴ� ���̹Ƿ�
                // �浹ü�� ������ ����� ������ �����ð� �� �����ϴ� �ڷ�ƾ �۵�
                StartCoroutine(FlatDelete()); 
            }
        }
    }

    /// <summary>
    /// HealFlat�� �������� �����ϴ� �ڷ���
    /// </summary>
    /// <returns></returns>
    IEnumerator FlatDelete()
    {
        float timeTerm = 0.3f; // �����̴� ������ �����ϴ� Ÿ�̸� - ���ϴ� �ð����� ���� ����
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

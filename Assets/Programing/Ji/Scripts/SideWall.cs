using System.Collections;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    [SerializeField] float poisionDamage; // �� �������� ũ��
    [SerializeField] float poisionDebuffTime; // �� ������ ����� �ֱ�
    // �浹�� �÷��̾��� �÷��̾� ��Ʈ�ѷ��� �����������ؼ� �ۼ�
    // �̸� ����Ƽ���� �־� ���� �͵� ��ĥ�� �����غ��� ���� ���� �� ����
    // �� ��� OnCollsionEnter2D�� samplePlayer = collision.gameObject.GetComponent<SamplePlayer>(); ������ ��
    [SerializeField] SamplePlayer samplePlayer; // �÷��̾��� ��ũ��Ʈ�� ���� �ٸ��� ������ ��

    bool isDebuff = false; // ������� Ȱ��ȭ ���� ����

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �÷��̾�� �浹�ϸ�
            isDebuff = true; // ����� Ȱ��ȭ
            // �浹�� �÷��̾��� �÷��̾� ��Ʈ�ѷ� �ҷ�����
            samplePlayer = collision.gameObject.GetComponent<SamplePlayer>();
            // �÷��̾��� ü���� �����ð� ������ �����ϴ� �ڷ�ƾ ����
            StartCoroutine(PoisonDebuff());
        }
    }

    /// <summary>
    /// ��ü�� �浹���� ���������� ��� �Ǵ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �÷��̾ ���������� ���
        if (collision.gameObject.tag == "Player")
        {
            isDebuff = false; // ����� ��Ȱ��ȭ
            StopCoroutine(PoisonDebuff()); // �ڷ�ƾ ����
        }
    }

    /// <summary>
    /// �� �������� ���� �ֱ⿡ ���缭 �� ������� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator PoisonDebuff()
    {
        while (isDebuff)
        {
            samplePlayer.TakeDamage(poisionDamage); // �÷��̾� ��Ʈ�ѷ��� TakeDamage�� �÷��̾� ü�� ����
            // �÷��̾��� ü�� ����
            yield return new WaitForSeconds(poisionDebuffTime); // ���� �ð� ����
        }
    }
}

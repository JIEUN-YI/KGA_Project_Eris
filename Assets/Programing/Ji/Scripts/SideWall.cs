using System.Collections;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    [SerializeField] float poisionDamage; // �� �������� ũ��
    [SerializeField] float poisionDebuffTime; // �� ������ ����� �ֱ�
    // �浹�� �÷��̾��� �÷��̾� ��Ʈ�ѷ��� �����������ؼ� �ۼ�
    // �̸� ����Ƽ���� �־� ���� �͵� ��ĥ�� �����غ��� ���� ���� �� ����
    // �� ��� OnCollsionEnter2D�� samplePlayer = collision.gameObject.GetComponent<SamplePlayer>(); ������ ��
    [SerializeField] SamplePlayer samplePlayer;

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
        while (true)
        {
            samplePlayer.TakeDamage(poisionDamage); // �÷��̾� ��Ʈ�ѷ��� TakeDamage�� �÷��̾� ü�� ����
            Debug.Log("������");
            // �÷��̾��� ü�� ����
            yield return new WaitForSeconds(poisionDebuffTime); // ���� �ð� ����
            Debug.Log("�ֱ� �ð� ����");
        }
    }

    /* OnCollisionStay2D�� ����Ͽ� �浹�ϴ� ����
     * �ڷ�ƾ�� �����Ͽ� �浹�� �÷��̾�� ������� �ο��Ϸ� ü���� �����ϰ�
     * OnCollisionExit2D�� ����Ͽ� �浹���� ����������
     * �ڷ�ƾ�� �����ϵ��� ����
     * - ����� ������� �ʾƼ� ��� ����
     * - �������� ����ؼ� ������Ʈ�Ǹ� �ֱ�� ���� �����
     
    [SerializeField] float poisionDamage;
    [SerializeField] float poisionDebuffTime;
    Coroutine PoisionDebuffR;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // �÷��̾�� �浹�ϴ� ���ȿ�
            // �÷��̾��� ü���� �����ϰ� ����
            SamplePlayer samplePlayer = collision.gameObject.GetComponent<SamplePlayer>();
            PoisionDebuffR = StartCoroutine(PoisonDebuff(samplePlayer));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StopCoroutine(PoisionDebuffR);
    }

    /// <summary>
    /// �� �������� ���� �ֱ⿡ ���缭 �� ������� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator PoisonDebuff(SamplePlayer samplePlayer)
    {
        while (true)
        {
        samplePlayer.TakeDamage(poisionDamage);
        Debug.Log("������");
        // �÷��̾��� ü�� ����
        yield return new WaitForSeconds(poisionDebuffTime);
        Debug.Log("�ֱ� �ð� ����");
        }
    }
    */
    /* bool ���� isDebuff�� ����Ͽ� ������� Ȱ��ȭ ���θ� �Ǵ��ϰ� 
     * ����� �ֱ⸦ �ڷ�ƾ���� �����ϴ� �ҽ��ڵ�
     * ��, �������� ������ ������ ������ �߻��Ͽ� ��� - ���� ���� ����
     *
    [Header("State")]
    [SerializeField] float poisionDebuffTime; // �� �������� ���� �ֱ�
    float poisionDamage = 2f; // �� ������
    SamplePlayer samplePlayer;

    bool isDebuff = false; // ������� �۵��ϴ��� ���θ� �Ǵ� - true = ����� Ȱ��ȭ


    private void Update()
    {
        if (isDebuff) // ������� �۵��ϰ� �ִ� ���̸�
        {
            StartCoroutine(PoisonDebuff()); // ����� �ֱ⸦ �����ϴ� �ڷ�ƾ Ȱ��ȭ
        }
        else if (!isDebuff) // ������� ����Ǹ�
        {
            StopCoroutine(PoisonDebuff()); // �ڷ�ƾ�� ����
        }
    }
    /// <summary>
    /// ���������� �浹 ���ε��� ���������� �߻��ϴ� �ൿ
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("�÷��̾�� �浹");
            isDebuff = true; // ������� Ȱ��ȭ
            samplePlayer = collision.gameObject.GetComponent<SamplePlayer>();
            // �÷��̾��� TakeDamage(); �Լ��� ������������ ����
            // StartCoroutine(PoisonDebuff());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDebuff = false; // ������� ����
            Debug.Log("�÷��̾� ü�� ����� ����");
        }
    }
    /// <summary>
    /// �� �������� ���� �ֱ⿡ ���缭 �� ������� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator PoisonDebuff()
    {
        samplePlayer.TakeDamage(poisionDamage);
        Debug.Log("������");
        // �÷��̾��� ü�� ����
        yield return new WaitForSeconds(poisionDebuffTime);
        Debug.Log("�ֱ� �ð� ����");
    }
    */
}

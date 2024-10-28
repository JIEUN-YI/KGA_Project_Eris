using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Phase1 : MonoBehaviour
{

    // ���� ���¿� ���߻��� 2���� ���󿡼� 2����+ �齺��  ���߿��� 2���� �������Ͽ��� 3������ �ٸ��������� ��ȯ
    // �����϶� �Ÿ�üũ�� �����϶� �Ÿ�üũ �� �̵������ �ٲ��� �� 

    //�齺��(��Ÿ������) , �������� ũ�� �ֵθ�, ���� , �ϴÿ��� ȭ���� �߻� , ���߿��� �����ϸ鼭 ������ ����, 2������ ����
    // ������ ��� �÷��̾ �����ϴϱ� �ϴ� ���̵��� �ʿ� ���� ��?
    enum BossState
    {
      Flying ,Walk , Attack, Die
    }
    // ���� �ִϸ��̼�
   // [SerializeField] Animator animator;
    // �÷��̾� ������
    [SerializeField] GameObject player;

    // ���� ����  : ü�� �̵��ӵ� 
    [SerializeField] int stateCount = 0;  // fly �� walk ���¿��� ī��Ʈ�� 2�� �Ǹ�(3�� ������) ���¸� �����Ҷ� ���� , ������ ���� �� �� ���� �ϳ��� ����

    public float bossHP = 10;
    [SerializeField] float attackRange;  // ��Ÿ� 
    [SerializeField] float bossSpeed;    // �̵��ӵ� 



    Coroutine curCoroutine;
    BossState state = BossState.Walk;
    [SerializeField] BossState curBossState; // ������ ���� ���� Ȯ�ο� 
    // �������� �������� �ַ��� BossPattern bossPattern = boss.GetComponent<BossPattern>();
    // bossPattern.TakeDamage(������);�� �������� �� �� ���� , �̺�Ʈ�� ȣ���ϸ� �ɵ�
    private void Start()
    {
        StartCoroutine("BossDo");
        curBossState = state;
    }
    IEnumerator BossDo() 
    {
        WaitForSeconds time = new WaitForSeconds(0.0125f);  // 1�ʿ� 80�� ȣ��
        curCoroutine = StartCoroutine(Walk());
        while (true)
        {
            Debug.Log("�ڷ�ƾ ������");
            yield return time;
            if (curBossState != state)
            {
                curBossState = state;

                // ���� ���� ���� �ڷ�ƾ�� ������ ����
                if (curCoroutine != null)
                {
                    StopCoroutine(curCoroutine);
                }

                // ���ο� ���¿� �´� �ڷ�ƾ�� ����
                switch (state)
                {
                    case BossState.Walk:
                        curCoroutine = StartCoroutine(Walk());
                        break;
                    case BossState.Flying:
                        curCoroutine = StartCoroutine(Flying());
                        break;
                    case BossState.Die:
                       // Die();
                        break;
                }
            }
            
            yield return null;
        }
    }


    IEnumerator Flying() 
    { 
        Debug.Log("�ö��� ������");  // ���� �ڵ�� �ʹ� �� �ϰ� �ö󰡼� �� ���� �ö󰥶� �ִϸ��̼��̳� ����Ʈ�� ������ �� ������ 
                                     // �����϶� �̵��ӵ��� �ٸ��� ��������?
        while (true) 
        {   
            
            Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, bossSpeed * Time.deltaTime),
            30f
            );
            transform.position = newPosition;
            // ���߿� ���� �ֱ� 

            if (stateCount >= 2)
            {
                stateCount = 0;
                state = BossState.Walk;
            }

            yield return null;
        }

    }

    IEnumerator Walk()
    {
        
        while (true)
        {
            Debug.Log("��ũ ������");
            Vector2 newPosition = new Vector2(
                Mathf.MoveTowards(transform.position.x, player.transform.position.x, bossSpeed * Time.deltaTime),
                transform.position.y
            );
            transform.position = newPosition;

            // ����� ���� �ֱ�   ���߿��� ���� �����ö� ���������� �� �������
            if (stateCount >= 2)
            {
                stateCount = 0;
                state = BossState.Flying;
            }

            yield return null;
        }
    }



    private void Die()
    {
        // hp ���� �Ҹ� �� ��� �ִϸ��̼� ���� �� ������ �Ҹ�

        // ��� �ִϸ��̼� 
        // animator.Play();

        // ������Ʈ ���� ó��
        //Destroy(gameObject, 2f);
    }

    public void TakeDamage(float damage)
    {
        bossHP -= damage;

        // ������ ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (bossHP <= 0)
        {
            state = BossState.Die;
        }
    }

    private void Mirrored()
    {
        // �÷��̾ ������ ���ʿ� ������ ������ ��������, �����ʿ� ������ �������� �ٶ󺸰� ����
        if (player.transform.position.x < transform.position.x)
        {
            // ������ ������ �ٶ󺸵��� ��
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // ������ �������� �ٶ󺸵��� ��
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

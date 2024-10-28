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
        Flying, Walk, Attack, Die
    }
    // ���� �ִϸ��̼�
    // [SerializeField] Animator animator;
    // �÷��̾� ������
    [SerializeField] GameObject player;

    // ���� ����  : ü�� �̵��ӵ� 
    [SerializeField] int stateCount = 0;  // fly �� walk ���¿��� ī��Ʈ�� 3�� �Ǹ�(3�� ������) ���¸� �����Ҷ� ���� , ������ ���� �� �� ���� �ϳ��� ����

    public float bossHP = 10;
    [SerializeField] float toPlayerDistance; // ������ �÷��̾��� �Ÿ� 
    [SerializeField] float walkAttackRange;  // �����Ÿ� 

    [SerializeField] float flyAttackRange;  // ���߻�Ÿ� 

    [SerializeField] float walkbossSpeed;    // �̵��ӵ� 
    [SerializeField] float flybossSpeed;    // ���� �̵��ӵ� 
    private bool isPatternOn = false;
    private BossState preState;


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

            toPlayerDistance = Vector2.Distance(player.transform.position, transform.position);
                
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
                    case BossState.Attack:
                        curCoroutine = StartCoroutine(Attack());
                        break;
                    case BossState.Die:
                        // Die();
                        break;
                }
            }

            yield return time;
        }
    }


    IEnumerator Flying()
    {
        // ���� �ڵ�� �ʹ� �� �ϰ� �ö󰡼� �� ���� �ö󰥶� �ִϸ��̼��̳� ����Ʈ�� ������ �� ������ 
        if (stateCount >= 3)
        {
            stateCount = 0;
            state = BossState.Walk;
            Debug.Log("������ȯ");
        }
        while (state == BossState.Flying)
        {
            preState = curBossState;
            Mirrored();
            Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, flybossSpeed * Time.deltaTime),
            30f
            );
            transform.position = newPosition;
          

            if (toPlayerDistance <= flyAttackRange)
            {
                if (isPatternOn == false)
                {
                    preState = curBossState;
                    state = BossState.Attack;
                    stateCount++;
                }
            }


            yield return null;
        }


    }

    IEnumerator Walk()
    {
        if (stateCount >= 3)
        {
            stateCount = 0;
            state = BossState.Flying;
            Debug.Log("������ȯ");
        }
        while (state == BossState.Walk)
        {

            preState = curBossState;
            Mirrored();
            Vector2 newPosition = new Vector2(
                Mathf.MoveTowards(transform.position.x, player.transform.position.x, walkbossSpeed * Time.deltaTime),
                transform.position.y
            );
            transform.position = newPosition;


            // ����� ���� �ֱ�   ���߿��� ���� �����ö� ���������� �� ������� ,���� ������ ����Ǹ� �̵� ��ƾ�� ���� �ؾ��ҵ�
            if (toPlayerDistance <= walkAttackRange)
            {
                if (isPatternOn == false)
                {
                    preState = state;
                    state = BossState.Attack;
                    stateCount++;
                }

            }


            yield return null;
        }

    }

    //�齺��(��Ÿ������) , �������� ũ�� �ֵθ�, ���� , �ϴÿ��� ȭ���� �߻� , ���߿��� �����ϸ鼭 ������ ����, 2������ ����
    IEnumerator Attack()
    {
        isPatternOn = true;
        //int x = Random.Range(0, 2);
        //switch (x)
        //{
        //    case 0:
        //        StartCoroutine("BackStep");
        //        break;
        //    case 1:
        //        StartCoroutine("Slash");
        //        break;
        //    case 2:
        //        StartCoroutine("Bodytacle");
        //        break;
        //}
        //int y = Random.Range(0, 3);
        //switch (x)
        //{
        //    case 0:
        //        StartCoroutine("FireBall");
        //        break;
        //    case 1:
        //        StartCoroutine("RushSlash");
        //        break;
        //}

        Debug.Log($"{preState}{stateCount}���� ����");
        yield return new WaitForSeconds(1f);

        state = preState;
        isPatternOn = false;
    }

    IEnumerator BackStep()
    {
        Debug.Log("�齺��");
        StopCoroutine("BossDo");
        yield return new WaitForSeconds(1f);
        StartCoroutine("BossDo");
    }
    IEnumerator Slash()
    {
        Debug.Log("����");
        StopCoroutine("BossDo");
        yield return new WaitForSeconds(1f);
        StartCoroutine("BossDo");
    }
    IEnumerator Bodytacle()
    {
        Debug.Log("����");
        StopCoroutine("BossDo");
        yield return new WaitForSeconds(1f);
        StartCoroutine("BossDo");
    }
    IEnumerator FireBall()
    {
        Debug.Log("ȭ����");
        StopCoroutine("BossDo");
        yield return new WaitForSeconds(1f);
        StartCoroutine("BossDo");
    }
    IEnumerator RushSlash()
    {
        Debug.Log("���ߵ�������");
        StopCoroutine("BossDo");
        yield return new WaitForSeconds(1f);
        StartCoroutine("BossDo");
    }
    // 2������?


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
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // ������ �������� �ٶ󺸵��� ��
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}

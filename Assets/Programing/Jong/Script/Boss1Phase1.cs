using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
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
    [SerializeField] Animator animator;

    [SerializeField] GameObject player;

    [SerializeField] Rigidbody2D bossRigidbody;
    [SerializeField] GameObject RushCollider;

    // ���� ����  : ü�� �̵��ӵ� 
    [SerializeField] int stateCount = 0;  // fly �� walk ���¿��� ī��Ʈ�� 3�� �Ǹ�(3�� ������) ���¸� �����Ҷ� ���� , ������ ���� �� �� ���� �ϳ��� ����

    [SerializeField] float bossHP = 10;
    [SerializeField] float toPlayerDistance; // ������ �÷��̾��� �Ÿ� 
    [SerializeField] float walkAttackRange;  // �����Ÿ� 

    [SerializeField] float flyAttackRange;  // ���߻�Ÿ� 

    [SerializeField] float walkbossSpeed;    // �̵��ӵ� 
    [SerializeField] float flybossSpeed;    // ���� �̵��ӵ� 
    private bool isPatternOn = false; // �������̸� 
    private BossState preState; // ���� �����ϱ� ���� ���� �����
    private bool isflying = false;  // ���� Ȯ�ο� 

    //���� ���Ͽ� �ʵ�
    [SerializeField] Transform atkPoint;
    [SerializeField] GameObject slashPrefap;
    [SerializeField] GameObject fireBallPrefab;
    [SerializeField] Transform[] fireBallPoints;

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

        // WaitForSeconds time = new WaitForSeconds(0.1f);  // 1�ʿ� 80�� ȣ��
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

            yield return null;
        }
    }


    IEnumerator Flying()
    {
        isflying = true;
        bossRigidbody.gravityScale = 0f; // ���߿��� ������  �� �̵��� ȣ����� �����ϱ� �߷��� 0���� ���� ��� ���ְ� �� 
        // ���� �ڵ�� �ʹ� �� �ϰ� �ö󰡼� �� ���� �ö󰥶� �ִϸ��̼��̳� ����Ʈ�� ������ �� ������ 
        if (stateCount >= 3)
        {
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(Fork());
            yield return new WaitForSeconds(1.5f);
            stateCount = 0;
            state = BossState.Walk;
            Debug.Log("������ȯ");
           
        }
        while (state == BossState.Flying)
        {
            preState = curBossState;
            Mirrored();
            animator.Play("Fly");
            Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, flybossSpeed * Time.deltaTime),
            10f
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
        isflying = false;
        bossRigidbody.gravityScale = 1f;
        if (stateCount >= 3)
        {
            animator.Play("Jump");
            yield return new WaitForSeconds(1.2f);
            stateCount = 0;
            state = BossState.Flying;
            Debug.Log("������ȯ");

        }
        while (state == BossState.Walk)
        {
            animator.Play("Walk");
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
        WaitForSeconds time = new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(1f);
        if (isflying == false)
        {
            animator.Play("WalkIdle");
        }
        else if (isflying == true) 
        {
            animator.Play("FlyIdle");
        }  
        if (isflying == false) // ���� ����
        {
            if (isPatternOn == false)
            {
                int x = Random.Range(0, 3);
                switch (x)
                {
                    case 0:
                        StartCoroutine(BackStep());
                        break;
                    case 1:
                        StartCoroutine(Slash());
                        break;
                    case 2:
                        StartCoroutine(BodyTacle());
                        break;
                }
            }
        }
        else if (isflying == true) // ���� ���� 
        {
            if (isPatternOn == false)
            {
                int y = Random.Range(0, 2);
                switch (y)
                {
                    case 0:
                        StartCoroutine(FireBall());
                        break;
                    case 1:
                        StartCoroutine(RushSlash());
                        break;
                }
            }
        }


        yield return time; // ��ų���� �� , ��ų�� ���� �ð��� �������� ������ ���� �� 

        state = preState;  // ���� ������ ���·� ���ư� 
        
    }
    IEnumerator BackStep()
    {
        isPatternOn = true;
        animator.Play("BackStep");
        Debug.Log("�齺��");
        yield return new WaitForSeconds(0.25f);
        if (player.transform.position.x < transform.position.x) // �÷��̾��� �ݴ� �������� ���ư� 
        {

            bossRigidbody.AddForce(Vector2.right * 50f, ForceMode2D.Impulse);
        }
        else
        {

            bossRigidbody.AddForce(Vector2.left * 50f, ForceMode2D.Impulse);
        }


        yield return new WaitForSeconds(0.25f);

        bossRigidbody.velocity = Vector2.zero; // �ʹ� �ȹи��� �ӵ� ���� 
        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator Slash()
    {
        isPatternOn = true;
        animator.Play("Atk1");
        yield return new WaitForSeconds(0.3f);
        Debug.Log("����");

        GameObject obj = Instantiate(slashPrefap, atkPoint.position, atkPoint.rotation);

        Destroy(obj, 0.25f);
        yield return new WaitForSeconds(1f);
        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);

    }
    IEnumerator BodyTacle()
    {
        isPatternOn = true;
        animator.Play("Atk2");
        RushCollider.SetActive(true);
        Debug.Log("����");
        yield return new WaitForSeconds(1f);
        if (player.transform.position.x < transform.position.x) // �÷��̾��� �������� ���ư� 
        {


            bossRigidbody.AddForce(Vector2.left * 150f, ForceMode2D.Impulse);
        }
        else
        {
            bossRigidbody.AddForce(Vector2.right * 150f, ForceMode2D.Impulse);

        }


        yield return new WaitForSeconds(0.8f);

        bossRigidbody.velocity = Vector2.zero; // �ʹ� �ȹи��� �ӵ� ���� 
        RushCollider.SetActive(false);

        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator FireBall()
    {
        isPatternOn = true;
        animator.Play("Atk3");
        yield return new WaitForSeconds(1f);
        Debug.Log("ȭ����");
        if (player.transform.position.x < transform.position.x)
        {
            for (int i = 0; i < fireBallPoints.Length; i++)
            {
                GameObject obj = Instantiate(fireBallPrefab, fireBallPoints[i].position, fireBallPoints[i].rotation);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.left * 50f, ForceMode2D.Impulse);
                Destroy(obj, 2.3f); 
            }

        }
        
        else
        {
            for (int i = 0; i < fireBallPoints.Length; i++)
            {
                GameObject obj = Instantiate(fireBallPrefab, fireBallPoints[i].position, fireBallPoints[i].rotation);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.right * 50f, ForceMode2D.Impulse);
                Destroy(obj, 2.3f);
            }

        }
        yield return new WaitForSeconds(0.5f);
        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator RushSlash()
    {
        isPatternOn = true;
        animator.Play("Atk4");
        Debug.Log("���ߵ�������");
        yield return new WaitForSeconds(0.3f);
        if (player.transform.position.x < transform.position.x) // �÷��̾��� �������� ���ư� 
        {

            for (int i = 0; i < 3; i++) 
            {
                yield return new WaitForSeconds(0.2f);
                bossRigidbody.AddForce(Vector2.left * 30f, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.3f);
                bossRigidbody.velocity = Vector2.zero;
                GameObject obj = Instantiate(slashPrefap, atkPoint.position, atkPoint.rotation);
                Destroy(obj, 0.25f);
                yield return new WaitForSeconds(0.3f);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                bossRigidbody.AddForce(Vector2.right * 30f, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.3f);
                bossRigidbody.velocity = Vector2.zero;
                GameObject obj = Instantiate(slashPrefap, atkPoint.position, atkPoint.rotation);
                Destroy(obj, 0.25f);
                yield return new WaitForSeconds(0.3f);
            }
        }
        yield return new WaitForSeconds(1f);
        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);

    }
    IEnumerator Fork() 
    {
        isPatternOn = true;
        yield return new WaitForSeconds(1f);
        
        bossRigidbody.AddForce(Vector2.down * 300f, ForceMode2D.Impulse);
        animator.Play("Landing");

        yield return new WaitForSeconds(0.25f);
        bossRigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(1f);
        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);
    }
    // 2������?


    private void Die()  // ����ϸ� 2������� ���� 
    {
        // hp ���� �Ҹ� �� ��� �ִϸ��̼� ���� �� ������ �Ҹ�

        // ��� �ִϸ��̼� 
        // animator.Play();

        // ������Ʈ ���� ó��
        //Destroy(gameObject, 2f);
    }

    public void TakeDamage(float damage) // ������Ʈ�� �̺�Ʈ�� ó���ϸ� �ɵ�
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

using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    enum BossState
    {
        Idle, Move, Attack, Die
    }
    // ���� ���� ������Ʈ
    [SerializeField] GameObject bossObject;
    // ���� �ִϸ��̼�
    [SerializeField] Animator animator;
    // �÷��̾� ������
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody2D bossRigid;

    // ���� ���� ���� bool
    private bool skillStart = false;

    // ��ų ����Ʈ ������


    // ���� ��Ÿ�
    [SerializeField] float attackRange;

    // ���� ����
    // ���� HP
    public float bossHP = 10;
    // ���� ���ǵ�
    [SerializeField] float bossSpeed;

    // ���� ���� ���� ����
    int bossPatternNum;
    // ���¸� Idle�� ����
    BossState state = BossState.Idle;

    // ����� ������, ���� �߰� �߰��� ������
    // �������� �⺻������ �÷��̾ ���� ���� ������ Attack ��Ȳ���� ����

    // ��ų���� ���� ��ų ����Ʈ ������ ���� 

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        switch (state)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Move:
                Move();
                break;
            case BossState.Attack:
                // ���� �߿��� �ٸ� ������ ���� �ʵ��� ��
                break;
            case BossState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        // Idle �ִϸ��̼�
        animator.Play("Testidel");

        // �÷��̾� ��ġ�� �ٶ󺸰�
        Mirrored();

        // �÷��̾���� �Ÿ� distanceToPlayer
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �÷��̾�� �Ÿ��� ����Ͽ� ���� ����
        if (playerDirection >= attackRange)
        {
            Debug.Log("Move ���·� ����");
            state = BossState.Move;
        }
        // ��ų ������ ������ ��������
        else if (skillStart == false)
        {
            StartCoroutine(ExecuteAttackPattern());
        }
    }
    private void Move()
    {
        // �޸��� �ִϸ��̼� ���
        animator.Play("TestRun");

        Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, bossSpeed * Time.deltaTime),
            transform.position.y
        );
        transform.position = newPosition;
        // �̵� �߿��� ��������Ʈ�� �÷��̾ �ٶ󺸰� ó��
        Mirrored();

        // ��Ÿ� ����
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // ��Ÿ� ���� ������ ���� ��ȯ
        if (playerDirection <= attackRange)
        {
            if (skillStart == false)
            {
                StartCoroutine(ExecuteAttackPattern());
            }           
        }
    }
    private IEnumerator ExecuteAttackPattern()
    {
        // ��ų ���� ����
        skillStart = true;

        // �������� ���� ���� ����

        // ���� �ѹ� ���� ����
        bossPatternNum = Random.Range(1, 5);

        //���� test �ڵ�
        bossPatternNum = 1;

        state = BossState.Attack;

        switch (bossPatternNum)
        {
            case 1:
                yield return StartCoroutine(BodyTackle());
                Debug.Log("BodyTackle ����");
                break;
            case 2:
                yield return StartCoroutine(Patton05());
                Debug.Log("Patton05");
                break;
            case 3:
                yield return StartCoroutine(Patton06());
                Debug.Log("Patton06");
                break;
            case 4:
                yield return StartCoroutine(Patton07());
                Debug.Log("Patton07");
                break;
        }
        Debug.Log("Idle ���·� ����");

    }
    private void Die()
    {
        // hp ���� �Ҹ� �� ��� �ִϸ��̼� ���� �� ������ �Ҹ�

        // ��� �ִϸ��̼� 
        // animator.Play();
        
        // ������Ʈ ���� ó��
        //Destroy(gameObject, 2f);
    }

    private IEnumerator BodyTackle()
    {
        // ���� ��ġ�� ����

        // �÷��̾� ��ġ Ž��
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        // ���� ���� ��ġ
        Vector2 startPosition = transform.position;

        // �ִϸ��̼� ���
        // ���ð�
        Debug.Log("���� ����---!");

        // ���� ��ǥ �Ÿ� ����
        float targetDistance = 30f; // ������ �̵��� �Ÿ� (���ϴ� ������ ����)
        float currentDistance = 0f;

        // ���� ����
        Debug.Log("���� ����---!");
        bossRigid.velocity = playerDirection * 70f; // ���ϴ� ���� �ӵ� ����

        while (currentDistance < targetDistance)
        {
            currentDistance = Vector2.Distance(transform.position, startPosition);

            if (currentDistance >= targetDistance)
            {
                // ��ǥ �Ÿ��� ���������Ƿ� ����
                bossRigid.velocity = Vector2.zero;
                bossRigid.AddForce(Vector2.zero, ForceMode2D.Impulse);
                break;
            }

            yield return null; // ���� �����ӱ��� ���
        }

        yield return new WaitForSeconds(5f);
        Debug.Log("���� ��---!");
        skillStart = false;
        state = BossState.Idle;
    }
    private IEnumerator Patton05()
    {
        // ���濡 ȭ���� �߻� ����

        // ȭ���� �߻� �ִϸ��̼�
        // ���ð�
        // ȭ���� ������ ���� 
        // ȭ������ �������� �̵�

        Debug.Log("�ҽ��---!");

        // �� �߻� ���� ���� ��� (1.5�� �� Idle ���·� ����)
        yield return new WaitForSeconds(1.5f);

        Debug.Log("�� �߻� ��---!");
        state = BossState.Idle;
    }
    private IEnumerator Patton06()
    {
        // ���� ���� �Ĺ����� ȭ�� ��� ������ ����

        // ��¡�ϴ� �ִϸ��̼�
        // ���ð�

        // ������ ��ҿ� ȭ����� ������ �ټ� ����

        Debug.Log("�ұ�� �����---!");

        // �ұ�� ���� ���� ���� ��� (2�� �� Idle ���·� ����)
        yield return new WaitForSeconds(2f);

        Debug.Log("�ұ�� ���� ��---!");
        state = BossState.Idle;
    }
    private IEnumerator Patton07()
    {
        //���� ������ ���� ����

        // �����ϴ� �ִϸ��̼�
        animator.Play("TestJump");
        // ���� ���� �ݶ��̴��� �ǰ�����

        Debug.Log("�������� ����---!");

        // ���� ���� ���� ���� ��� (3�� �� Idle ���·� ����)
        yield return new WaitForSeconds(3f);

        Debug.Log("�������� ��---!");
        state = BossState.Idle;
    }

    // �������� �������� �ַ��� BossPattern bossPattern = boss.GetComponent<BossPattern>();
    // bossPattern.TakeDamage(������);�� �������� �� �� ����
    public void TakeDamage(float damage)
    {
        bossHP -= damage;

        // ������ ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (bossHP <= 0)
        {
            state = BossState.Die;
        }
    }

    // �¿����
    private void Mirrored()
    {
        // �÷��̾ ������ ���ʿ� ������ ������ ��������, �����ʿ� ������ �������� �ٶ󺸰� ����
        if (player.transform.position.x < bossObject.transform.position.x)
        {
            // ������ ������ �ٶ󺸵��� ��
            bossObject.transform.localScale = new Vector3(-1, bossObject.transform.localScale.y, bossObject.transform.localScale.z);
        }
        else
        {
            // ������ �������� �ٶ󺸵��� ��
            bossObject.transform.localScale = new Vector3(1, bossObject.transform.localScale.y, bossObject.transform.localScale.z);
        }
    }
}

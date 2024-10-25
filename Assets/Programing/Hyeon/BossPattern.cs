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
                Attack();
                break;
            case BossState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        // Idle �ִϸ��̼�
        // animator.Play();

        // �÷��̾� ��ġ�� �ٶ󺸰�
        Mirrored();

        // �÷��̾���� �Ÿ� distanceToPlayer
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // �÷��̾�� �Ÿ��� ����Ͽ� ���� ����
        if (distanceToPlayer > attackRange)
        {
            state = BossState.Move;
        }
        else
        {
            state = BossState.Attack;
        }
        /*if (�÷��̾���� �Ÿ��� �����Ÿ� �̻��̸�)
         *������ ���� Move�� ����
         * state = BossState.Move;
         * 
         * else �ƴ϶��
         * ������ ���� Attack���� ����
         * state = BossState.Attack;
        */

    }
    private void Move()
    {
        Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, bossSpeed * Time.deltaTime),
            transform.position.y
        );
        transform.position = newPosition;
        // �̵� �߿��� ��������Ʈ�� �÷��̾ �ٶ󺸰� ó��
        Mirrored();

        // ��Ÿ� ���� ������ ���� ���·� ��ȯ
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer <= attackRange)
        {
            state = BossState.Attack;
        }
    }
    private void Attack()
    {
        // �������� ���� ���� ����

        // ���� �ѹ� ���� ����
        bossPatternNum = Random.Range(1, 5);

        switch (bossPatternNum)
        {
            case 1:
                Patton02();
                break;
            case 2:
                Patton05();
                break;
            case 3:
                Patton06();
                break;
            case 4:
                Patton07();
                break;
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

    public void Patton02()
    {
        // ���� ��ġ�� ����
        // �ִϸ��̼� ���
        // ���ð�
        // �������� ����

        // Idle() ���·� ����
        state = BossState.Idle;
    }
    public void Patton05()
    {
        // ���濡 ȭ���� �߻� ����

        // ȭ���� �߻� �ִϸ��̼�
        // ���ð�
        // ȭ���� ������ ���� 
        // ȭ������ �������� �̵�

        // Idle() ���·� ����
        state = BossState.Idle;
    }
    public void Patton06()
    {
        // ���� ���� �Ĺ����� ȭ�� ��� ������ ����

        // ��¡�ϴ� �ִϸ��̼�
        // ���ð�

        // ������ ��ҿ� ȭ����� ������ �ټ� ����

        // Idle() ���·� ����
        state = BossState.Idle;
    }
    public void Patton07()
    {
        //���� ������ ���� ����

        // �����ϴ� �ִϸ��̼�
        // ���� ���� �ݶ��̴��� �ǰ�����

        // Idle() ���·� ����
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
            bossObject.transform.localScale = new Vector3(1, bossObject.transform.localScale.y, bossObject.transform.localScale.z);
        }
        else
        {
            // ������ �������� �ٶ󺸵��� ��
            bossObject.transform.localScale = new Vector3(-1, bossObject.transform.localScale.y, bossObject.transform.localScale.z);
        }
    }
}

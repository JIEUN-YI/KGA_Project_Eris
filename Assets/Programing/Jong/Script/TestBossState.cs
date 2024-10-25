using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossState : MonoBehaviour
{
    enum BossState
    {
        Idle, Move, Attack, Die
    }
    // ���� ���� ������Ʈ
    [SerializeField] GameObject bossObject;


    [SerializeField] Rigidbody2D bossRigid;
    [SerializeField] PolygonCollider2D bossPolygonCollider;
    // ���� �ִϸ��̼�
   // [SerializeField] Animator animator;
    // �÷��̾� ������
    [SerializeField] GameObject player;
    // ��ų ����Ʈ ������



    // ���� ����
    public float bossHP = 10;

    // ���� ���� ���� ����
    int bossPatternNum;
    // ���¸� Idle�� ����
    BossState state = BossState.Idle;
    // ����� ������, ���� �߰� �߰��� ������
    // �������� �⺻������ �÷��̾ ���� ���� ������ Attack ��Ȳ���� 

    // ��ų ���� ���� ��ų ����Ʈ ������ ���� 

    [SerializeField] BossState curBossState;
    private void Awake()
    {
      
    }


    private void Update()
    {   
        curBossState = state;
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

        if (bossHP <= 0f)
        {
            state = BossState.Die;
        }
    }

    private void Idle()
    {
        // Idle �ִϸ��̼�
        // animator.Play();

        /* if (�÷��̾���� �Ÿ��� �����Ÿ� �̻��̸�)
         * ������ ���� Move�� ����
         * state = BossState.Move;
         * 
         * else �ƴ϶��
         * ������ ���� Attack���� ����
         * state = BossState.Attack;
        */

       // Debug.Log(Vector2.Distance(player.transform.position, transform.position));
        if (Vector2.Distance(player.transform.position, transform.position) < 7f) 
        {
            Debug.Log("���� ���·� ���� ");
            state = BossState.Attack;
        }
    }
    private void Move()
    {
        // �÷��̾� ��ġ�� �̵�

        // �÷��̾���� ��ġ�� �����ٸ�
        // state = BossState.Attack;
    }
    private void Attack()
    {
        // �������� ���� ���� ����
        // ���� �ѹ� ���� ����

        Patton02();
        //bossPatternNum = Random.Range(1, 5);

        //switch (bossPatternNum)
        //{
        //    case 1:
        //        Patton02();
        //        break;
        //    case 2:
        //        Patton05();
        //        break;
        //    case 3:
        //        Patton06();
        //        break;
        //    case 4:
        //        Patton07();
        //        break;
        //}
    }
    private void Die()
    {
        // hp ���� �Ҹ� �� ��� �ִϸ��̼� ���� �� ������ �Ҹ�

        // ��� �ִϸ��̼� 
        // animator.Play();

        // ������Ʈ ���� ó��
        //Destroy(gameObject, 2f);
    }
    // �齺��(��Ÿ������) , �������� ũ�� �ֵθ�, ���� , �ϴÿ��� ȭ���� �߻� , ���߿��� �����ϸ鼭 ������ ���� , 2������ ���� 
    public void Patton02()
    {
        // ���� ��ġ�� ����
        // �ִϸ��̼� ���
        // ���ð�
        // �������� ����
        bossRigid.AddForce(Vector2.left * 100f * Time.deltaTime, ForceMode2D.Impulse);
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
}

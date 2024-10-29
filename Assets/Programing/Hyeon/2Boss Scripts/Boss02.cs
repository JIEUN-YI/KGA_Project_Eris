using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02 : MonoBehaviour
{
    enum BossState
    {
        Idle, Move, Attack, Die, win
    }

    // ���� ���� ������Ʈ
    [SerializeField] GameObject bossObject;

    // �÷��̾� ������
    [SerializeField] GameObject player;
    // ������ Rigidbody
    [SerializeField] Rigidbody2D bossRigid;

    // ���� ���� ���� bool
    private bool skillStart = false;
    // �� �浹 ����
    private bool isWall = false;

    // ���� ����
    // ���� HP
    [SerializeField] float bossHP = 10;
    // ���� ���� ��Ÿ�
    [SerializeField] float attackRange;
    // ���� ���ǵ�
    [SerializeField] float bossSpeed;

    // �÷��̾� ��ġ
    Vector2 playerPosition;
    // ���� ���� ���� ����
    int bossPatternNum;
    // ���� ���� ����
    int bosscount;

    // ���¸� Idle�� ����
    BossState state = BossState.Idle;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        bossRigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (state)
        {
            case BossState.Idle:
                StartCoroutine(Idle());
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
            case BossState.win:
                Win();
                break;
        }
    }

    private IEnumerator Idle()
    {
        Debug.Log("���");
        skillStart = false;

        // ��� �ִϸ��̼�
        // animator.Play("");

        // �÷��̾� ��ġ�� �ٶ󺸰�
        Mirrored();

        // ��ų ���̿� ��� ��� �������� Ȱ��ȭ
        yield return new WaitForSeconds(1f);

        // �÷��̾���� �Ÿ� playerDirection
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �÷��̾�� �Ÿ��� ����Ͽ� ���� ����
        if (playerDirection >= attackRange)
        {
            //�Ÿ��� ��Ÿ����� �ָ� Move ���·� ����
            state = BossState.Move;
        }
        // ��ų ������ ������ ��������
        else if (!skillStart)
        {
            // �÷��̾���� �Ÿ��� ��Ÿ����� �������� �����غ�
            StartCoroutine(WaitSkill());
        }
    }


    private void Move()
    {
        Debug.Log("MOVE");
        // �޸��� �ִϸ��̼� ���
        // animator.Play("");

        // ������ ���� �ڵ�
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
                StartCoroutine(WaitSkill());
            }
        }
    }
    private IEnumerator WaitSkill()
    {
        // ��ų ���� ����
        skillStart = true;
        // ��� �ִϸ��̼�
        // animator.Play("boss1 2 idel");

        // ���� ����
        state = BossState.Attack;

        // �÷��̾� ���� ����
        playerPosition = (player.transform.position - transform.position).normalized;

        yield return new WaitForSeconds(2f);

        // �÷��̾� �Ÿ� ���
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �Ÿ��� ���� ������ ���� ��ȭ
        if (playerDirection <= attackRange / 3)
        {
            // Debug.Log($"ª�� �Ÿ�");
            // 1,2 �� ����
            bossPatternNum = 1;
        }
        else
        {
            //Debug.Log($"�� �Ÿ�");
            // 3,4 �� ����
            bossPatternNum = 2;
        }

        StartCoroutine(ExecuteAttackPattern());
    }
    
    private IEnumerator ExecuteAttackPattern()
    {
        if (bosscount == 3)
        {
            bossPatternNum = Random.Range(2, 4);
            bosscount = 0;
        }
        skillStart = true;
        switch (bossPatternNum)
        {
            case 1:
                yield return StartCoroutine(Bash());
                Debug.Log("���� ����");
                break;
            case 2:
                yield return StartCoroutine(FootWork());
                Debug.Log("�ߵ� ����");
                break;
            case 3:
                yield return StartCoroutine(Bash());
                Debug.Log("���� �˱� ������");
                break;
        }
        skillStart = false;
        state = BossState.Idle;
    }

    private IEnumerator Bash()
    {
        // �Ϲݺ���
        bosscount += 1;
        Debug.Log("�������!");
        yield return new WaitForSeconds(2f);
    }
    private IEnumerator FootWork()
    {
        // �ߵ� 
        isWall = false;

        // ���� ���� ��ġ
        Vector2 startPosition = transform.position;
        // ���� ���� ����
        Vector2 dashDirection;
        if (bossObject.transform.localScale.x > 0)
        {
            dashDirection = Vector2.right;
        }
        else
        {
            dashDirection = Vector2.left;
        }
        // �����̵� �Ÿ�
        float targetDistance = 50f;
        // Į �̴� �ִϸ��̼� ���
        //animator.Play("boss1 2 BodyTackle");
        // �ִϸ��̼� ��� �ð�
        yield return new WaitForSeconds(0.7f);
        // �����ɽ�Ʈ ���
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, targetDistance, LayerMask.GetMask("Wall"));

        Vector2 targetPosition;
        // �����ɽ�Ʈ�� ���� �������
        if (hit.collider != null)
        {
            // ���� ������ ������ �� �ձ����� �̵�
            targetPosition = (Vector2)transform.position + dashDirection * (hit.distance - 1f);
        }
        else
        {
            // ���� ������ ������ �Ÿ���ŭ �����̵�
            targetPosition = (Vector2)transform.position + dashDirection * targetDistance;
        }
        // ���� ��ġ
        transform.position = targetPosition;
        // Į �ִ� �ִϸ��̼� ���
        //animator.Play("boss1 2 BodyTackle");

        // �ܻ� ������ ����

        yield return new WaitForSeconds(0.7f);
    }
    // ������ ��� �Լ�
    public void TakeDamage(float damage)
    {
        bossHP -= damage;

        // ������ ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (bossHP <= 0)
        {
            state = BossState.Die;
        }
    }
    private void Die()
    {
        // hp ���� �Ҹ� �� ��� �ִϸ��̼� ���� �� ������ �Ҹ�

        // ��� �ִϸ��̼� 
        //animator.Play("");

        // ������Ʈ ���� ó��
        Destroy(gameObject, 4f);
    }
    private void Win()
    {
        // �÷��̾��� HP�� 0�̵Ǿ��ų� ���°� DIE�� �Ǿ�����

        // �¸� �ִϸ��̼�
        //animator.Play("boss1 2 win");
    }
    // �¿����
    private void Mirrored()
    {
        // �÷��̾ ������ ���ʿ� ������ ������ ��������, �����ʿ� ������ �������� �ٶ󺸰� ����
        if (player.transform.position.x < bossObject.transform.position.x)
        {
            Debug.Log("���� ����");
            // ������ ������ �ٶ󺸵��� ��
            bossObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            Debug.Log("���� ������");
            // ������ �������� �ٶ󺸵��� ��
            bossObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

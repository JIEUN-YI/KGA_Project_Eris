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
    // SwordAura �˱� ������
    [SerializeField] GameObject swordAura;
    // ���� ���� ���� bool
    private bool skillStart = false;
    // SwordAura �˱� ���� ��ǥ
    [SerializeField] Transform swordAuraPoint;
    // ���� ����
    // ���� HP
    [SerializeField] float bossHP = 10;
    // ���� ���� HP
    float bossNowHP;
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
        bossNowHP = bossHP;
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

        yield return new WaitForSeconds(1.5f);

        // �÷��̾� �Ÿ� ���
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �Ÿ��� ���� ������ ���� ��ȭ
        if (playerDirection <= attackRange / 3)
        {
            // Debug.Log($"ª�� �Ÿ�");
            // 1,2 �� ����
            bossPatternNum = 1;
        }
        else if (bossNowHP <= bossHP / 2)
        {
            //Debug.Log($"�� �Ÿ�");
            // 3,4 �� ����
            bossPatternNum = Random.Range(2, 4); ;
        }
        else
        {
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
        bossPatternNum = 3;
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
                yield return StartCoroutine(SkySwordAura());
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
        // ���� �ִϸ��̼�
        // ����Ʈ ������ ����
        yield return new WaitForSeconds(2f);
    }
    private IEnumerator FootWork()
    {
        // �ߵ�
        // ���� ������ Y ��ǥ ����
        float y = transform.position.y;
        
        // �����̵� �Ÿ�
        float targetDistance = 100f;
        // Į �̴� �ִϸ��̼� ���
        //animator.Play("boss1 2 BodyTackle");
        // �ִϸ��̼� ��� �ð�
        yield return new WaitForSeconds(0.7f);
        // �����ɽ�Ʈ ���
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerPosition, targetDistance, LayerMask.GetMask("Wall"));

        Vector2 targetPosition;
        // �����ɽ�Ʈ�� ���� �������
        if (hit.collider != null)
        {
            // ���� ����� ��, �� �ձ��� �̵�
            targetPosition = new Vector2(hit.point.x - playerPosition.x * 1f, y);
        }
        else
        {
            // ���� ������ ������ �Ÿ���ŭ �����̵�
            targetPosition = new Vector2(transform.position.x + playerPosition.x * targetDistance, y);
        }
        // ���� ��ġ
        transform.position = targetPosition;
        // Į �ִ� �ִϸ��̼� ���
        //animator.Play("boss1 2 BodyTackle");

        // �ܻ� ������ ����

        yield return new WaitForSeconds(0.7f);
    }
    // ������ ��� �Լ�
    private IEnumerator SkySwordAura()
    {
        float bossJumpPower = 110f;

        // animator.Play("���� ���� �ִϸ��̼�");     

        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.velocity = Vector2.zero;
        // ������ ���� �ö�
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);

        // �ö󰡰� ��¡�ϴ� �ִϸ��̼�
        //animator.Play("boss1 2 FireBarrier");

        // �ִϸ��̼� �� �ö󰡴� �ð��� ���� ���ð�
        yield return new WaitForSeconds(0.2f);
        // ������ ��ġ ����
        bossRigid.velocity = Vector2.zero;
        bossRigid.bodyType = RigidbodyType2D.Kinematic;

        // ������ �Ѹ��� �˱� ����
        for (int i = 0; i < 5; i++)
        {
            Mirrored();
            SwordAuraSpon();
            yield return new WaitForSeconds(1f);
        }

        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.gravityScale = 10;
        yield return new WaitForSeconds(2f);
        bossRigid.gravityScale = 1;
    }
    public void TakeDamage(float damage)
    {
        bossNowHP -= damage;

        // ������ ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (bossNowHP <= 0)
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
            bossObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            bossObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void SwordAuraSpon()
    {     
        GameObject swordSopn = Instantiate(swordAura, swordAuraPoint.position, Quaternion.identity);
        swordSopn.transform.LookAt(player.transform);
        SwordAura type = swordSopn.GetComponentInChildren<SwordAura>();

        // �÷��̾ ������ �¿� �ִ��� �쿡 �ִ��� �Ǵ�
        if (player.transform.position.x <= bossObject.transform.position.x)
        {
            type.direction = -1;
        }
        else
        {
            type.direction = 1;
        }
    }
}

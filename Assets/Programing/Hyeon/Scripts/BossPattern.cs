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
    // ������ Rigidbody
    [SerializeField] Rigidbody2D bossRigid;

    // ���̾ ���� ��ǥ
    [SerializeField] Transform fireBallPoint;
    // ���̾ ������
    [SerializeField] GameObject fireBallPre;

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
        bossRigid = GetComponent<Rigidbody2D>();
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

        // �÷��̾���� �Ÿ� playerDirection
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �÷��̾�� �Ÿ��� ����Ͽ� ���� ����
        if (playerDirection >= attackRange)
        {
            Debug.Log("Move ���·� ����");
            state = BossState.Move;
        }
        // ��ų ������ ������ ��������
        else if (!skillStart)
        {
            StartCoroutine(WaitSkill());
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
                StartCoroutine(WaitSkill());
            }
        }                   
    }

    // �Ÿ� ��� ����
    private IEnumerator WaitSkill()
    {
        // ��ų ���� ����
        skillStart = true;
        // ��� �ִϸ��̼�
        animator.Play("Testidel");

        // ���� ����
        state = BossState.Attack;


        yield return new WaitForSeconds(1f);

        // �÷��̾� �Ÿ� ���
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �Ÿ��� ���� ������ ���� ��ȭ
        if (playerDirection <= attackRange / 2)
        {
            Debug.Log($"ª�� �Ÿ�");
            // 1,2 �� ����
            bossPatternNum = Random.Range(1, 3);
        }
        else
        {
            Debug.Log($"�� �Ÿ�");
            // 3,4 �� ����
            bossPatternNum = Random.Range(3, 5);
        }

        StartCoroutine(ExecuteAttackPattern());
    }
    private IEnumerator ExecuteAttackPattern()
    {
        skillStart = true;
        // �������� ���� ���� ����

        //���� test �ڵ� ***���߿� ������!***
        //bossPatternNum = 3;

        switch (bossPatternNum)
        {
            case 1:
                yield return StartCoroutine(BodyTackle());
                Debug.Log("�ٵ���Ŭ ����");
                break;
            case 2:
                yield return StartCoroutine(JumpSlash());
                Debug.Log("�������� ����");
                break;
            case 3:
                yield return StartCoroutine(SponFireBall());
                Debug.Log("ȭ���� ����");
                break;
            case 4:
                yield return StartCoroutine(Patton06());
                Debug.Log("ȭ����� ����");
                break;           
        }
        skillStart = false;
        state = BossState.Idle;
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
        animator.Play("TestJump");

        // ���� ��ǥ �Ÿ� ����
        float targetDistance = 30f; // ������ �̵��� �Ÿ� (���ϴ� ������ ����)
        float currentDistance = 0f; // ���� �̵��Ÿ�
        float tackleSpeed = 70f;  // ���� �ӵ�

        // ���� ����
        Debug.Log("���� ����---!");

        // while������ ���� �Ÿ��� ����
        while (currentDistance < targetDistance)
        {
            // �浹 �����ϰ� ��ǥ ��ġ�� �̵�
            bossRigid.MovePosition(bossRigid.position + playerDirection * tackleSpeed * Time.deltaTime);
            currentDistance = Vector2.Distance(transform.position, startPosition);

            if (currentDistance >= targetDistance)
            {
                bossRigid.velocity = Vector2.zero;
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(3f);
        Debug.Log("���� ��---!");      
    }
    private IEnumerator JumpSlash()
    {
        //���� ������ ���� ����
        // ������ �Ŀ� 
        float bossJumpPower = 20f;

        // �����ϴ� �ִϸ��̼�
        animator.Play("TestJump");

        Debug.Log("�������� ����---!");
        // ���� ���� 
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        
        // ���� ���� �ݶ��̴��� �ǰ�����
  
        // ���� ���� ���� ���� ��� (3�� �� Idle ���·� ����)
        yield return new WaitForSeconds(3f);

        Debug.Log("�������� ��---!");

        // ���� ���� ���ϸ� ������ �ö󰡼� ������ ������
        // ���߿� ��ȹ�� �ǵ�� �ް� �ǵ��� �´��� QnA
    }

    private IEnumerator SponFireBall()
    {
        // ���濡 ȭ���� �߻� ����

        // ȭ���� �߻� �ִϸ��̼�
        animator.Play("TestJump");
        // �ִϸ��̼� ����ð�
        yield return new WaitForSeconds(4f);

        // ȭ���� ������ ����
        FireBallFire();
        // ȭ������ �������� �̵�

        Debug.Log("�ҽ��---!");

        // �� �߻� ���� ���� ��� (1.5�� �� Idle ���·� ����)
        yield return new WaitForSeconds(2f);

        Debug.Log("�� �߻� ��---!");
    }
    private IEnumerator Patton06()
    {
        // ������ ���� ���� �ʿ�
        // ���� ���� �Ĺ����� ȭ�� ��� ������ ����

        // �ö󰡰� ��¡�ϴ� �ִϸ��̼�
        animator.Play("TestJump");
        // ������ ���� �ö�

        // �ִϸ��̼� �� �ö󰡴� �ð��� ���� ���ð�
        yield return new WaitForSeconds(2f);
        // ������ ��ҿ� ȭ����� ������ �ټ� ����

        Debug.Log("�ұ�� �����---!");

        // �ұ�� ���� ���� ���� ��� (2�� �� Idle ���·� ����)
        yield return new WaitForSeconds(2f);

        Debug.Log("�ұ�� ���� ��---!");
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

    // ȭ���� �߻� 
    public void FireBallFire()
    {
        GameObject fireBall = Instantiate(fireBallPre, fireBallPoint.position, fireBallPoint.rotation);
    }
}

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

    //ȭ����� ���� ��ǥ
    [SerializeField] Transform fireWallPoint;
    // ȭ����� ������
    [SerializeField] GameObject fireWallPre;

    // ���� ���� ���� bool
    private bool skillStart = false;
    // �� �浹 ����
    private bool isWall = false;
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
    // �÷��̾� ��ġ
    Vector2 playerPosition;

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
        skillStart = false ;
        // Idle �ִϸ��̼�
        animator.Play("boss1_2_idel");
        
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
        animator.Play("boss1_2_idel");

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
        animator.Play("boss1_2_idel");

        // ���� ����
        state = BossState.Attack;

        // �÷��̾� ���� ����
        playerPosition = (player.transform.position - transform.position).normalized;

        yield return new WaitForSeconds(2f);

        // �÷��̾� �Ÿ� ���
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �Ÿ��� ���� ������ ���� ��ȭ
        if (playerDirection <= attackRange / 1.5)
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
        bossPatternNum = 4;

        switch (bossPatternNum)
        {
            case 1:             
                yield return StartCoroutine(FireBarrier());
                Debug.Log("ȭ����� ����");
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

                yield return StartCoroutine(BodyTackle());
                Debug.Log("�ٵ���Ŭ ����");
                break;           
        }
        skillStart = false;
        state = BossState.Idle;
        yield return new WaitForSeconds(1f);
        
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
        isWall = false;

        // ���� ���� ��ġ
        Vector2 startPosition = transform.position;

        // �ִϸ��̼� ���
        animator.Play("boss1_2_idel");

        // ���� ��ǥ �Ÿ� ����
        float targetDistance = 50f; // ������ �̵��� �Ÿ� (���ϴ� ������ ����)
        float currentDistance = 0f; // ���� �̵��Ÿ�
        float tackleSpeed = 120f;  // ���� �ӵ�

        // ���� ����
        Debug.Log("���� ����---!");

        // while������ ���� �Ÿ��� ����
        while (currentDistance < targetDistance && !isWall)
        {
            // �浹 �����ϰ� ��ǥ ��ġ�� �̵�
            Vector2 targetPosition = new Vector2(bossRigid.position.x + (playerPosition.x * tackleSpeed * Time.deltaTime), bossRigid.position.y);
            bossRigid.MovePosition(targetPosition);
            currentDistance = Vector2.Distance(new Vector2(transform.position.x, startPosition.y), startPosition);


            yield return null;
        }
        bossRigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(3f);
        Debug.Log("���� ��---!");      
    }
    private IEnumerator JumpSlash()
    {
        //���� ������ ���� ����
        // ������ �Ŀ� 
        float bossJumpPower = 30f;

        // �����ϴ� �ִϸ��̼�
        animator.Play("boss1_2_idel");

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
        animator.Play("boss1_2_idel");
        // �ִϸ��̼� ����ð�
        yield return new WaitForSeconds(4f);

        // ȭ���� ������ ����
        FireBallFire();
        // ȭ������ �������� �̵�

        // �� �߻� ���� ���� ��� (1.5�� �� Idle ���·� ����)
        yield return new WaitForSeconds(2f);

    }
    private IEnumerator FireBarrier()
    {
        float bossJumpPower = 20f;
        
        
        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        // ������ ���� �ö�
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        // �ö󰡰� ��¡�ϴ� �ִϸ��̼�
        animator.Play("boss1_2_idel");
        yield return new WaitForSeconds(1f);
        // ������ ��ġ ����
        bossRigid.bodyType = RigidbodyType2D.Kinematic;

        // �ִϸ��̼� �� �ö󰡴� �ð��� ���� ���ð�
        yield return new WaitForSeconds(1f);
        
      
        // ������ ������ ȭ�� �庮 ����
        FireWallInstant();
        Debug.Log("�ұ�� �����---!");

        // �ұ�� ���� ���� ���� ���
        yield return new WaitForSeconds(2f);

        bossRigid.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(1f);

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
            bossObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // ������ �������� �ٶ󺸵��� ��
            bossObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // ȭ���� �߻� 
    public void FireBallFire()
    {
        GameObject fireBall = Instantiate(fireBallPre, fireBallPoint.position, fireBallPoint.rotation);
    }

    public void FireWallInstant()
    {
        GameObject fireWall = Instantiate(fireWallPre, fireWallPoint.position, fireWallPoint.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� �浹�ߴ��� Ȯ�� Test => wall�� ��ü
        if (collision.gameObject.CompareTag("Test"))
        {
            isWall = true;
            Debug.Log("�浹");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Test"))
        {
            isWall = false;
        }
    }
}

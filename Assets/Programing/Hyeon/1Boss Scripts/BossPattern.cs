using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    enum BossState
    {
        Idle, Move, Attack, Die, win
    }
    // ���� ���� ������Ʈ
    [SerializeField] GameObject bossObject;
    // ���� �ִϸ��̼�
    [SerializeField] Animator animator;
    // �÷��̾� ������
    [SerializeField] GameObject player;
    // ������ Rigidbody
    [SerializeField] Rigidbody2D bossRigid;
    // ���� ���� collider
    [SerializeField] GameObject bossTacklePoint;
    // ���̾ ���� ��ǥ
    [SerializeField] Transform fireBallPoint;
    // ���̾ ������ ������
    [SerializeField] GameObject fireBallRightPre;
    // ���̾ ���� ������
    [SerializeField] GameObject fireBallLeftPre;

    //ȭ����� ���� ��ǥ
    [SerializeField] Transform fireWallPoint;
    // ȭ����� ������
    [SerializeField] GameObject fireWallPre;
    // �������� ����Ʈ ������ ������
    [SerializeField] GameObject slash;

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
            case BossState.win:
                Win();
                break;
        }
    }

    private void Idle()
    {
        skillStart = false ;
        // Idle �ִϸ��̼�
        animator.Play("boss1 2 idel");
        
        // �÷��̾� ��ġ�� �ٶ󺸰�
        Mirrored();

        // �÷��̾���� �Ÿ� playerDirection
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �÷��̾�� �Ÿ��� ����Ͽ� ���� ����
        if (playerDirection >= attackRange)
        {
            //Move ���·� ����
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
        animator.Play("boss1 2 walk");

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
        animator.Play("boss1 2 idel");

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
            // Debug.Log($"ª�� �Ÿ�");
            // 1,2 �� ����
            bossPatternNum = Random.Range(1, 3);
        }
        else
        {
            //Debug.Log($"�� �Ÿ�");
            // 3,4 �� ����
            bossPatternNum = Random.Range(3, 5);
        }

        StartCoroutine(ExecuteAttackPattern());
    }
    private IEnumerator ExecuteAttackPattern()
    {
        skillStart = true;

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
        animator.Play("boss1 2 die");
        
        // ������Ʈ ���� ó��
        Destroy(gameObject, 4f);
    }

    private void Win()
    {
        // �÷��̾��� HP�� 0�̵Ǿ��ų� ���°� DIE�� �Ǿ�����

        // �¸� �ִϸ��̼�
        animator.Play("boss1 2 win");
    }

    private IEnumerator BodyTackle()
    {
        // ���� ��ġ�� ����
        isWall = false;

        // ���� ���� ��ġ
        Vector2 startPosition = transform.position;
        // ���� collider Ȱ��ȭ
        bossTacklePoint.SetActive(true);
        // �ִϸ��̼� ���
        animator.Play("boss1 2 BodyTackle");

        yield return new WaitForSeconds(0.7f);
        // ���� ��ǥ �Ÿ� ����
        float targetDistance = 80f; // ������ �̵��� �Ÿ�
        float currentDistance = 0f; // ���� �̵��Ÿ�
        float tackleSpeed = 200f;  // ���� �ӵ�


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

        yield return new WaitForSeconds(0.7f);
        // ���� collider ��Ȱ��ȭ
        bossTacklePoint.SetActive(false);
        
    }
    private IEnumerator JumpSlash()
    {
        //���� ������ ���� ����
        // ������ �Ŀ� 
        float bossJumpPower = 15f;
        
        // �����ϴ� �ִϸ��̼�
        animator.Play("boss1 2 JumpSlash");
        yield return new WaitForSeconds(0.5f);
        // ���� ���� 
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.7f);
        bossRigid.gravityScale = 5;
        // ���� ���� �ݶ��̴��� �ǰ�����
        SlashEffect();
        // ���� ���� ���� ���� ��� (3�� �� Idle ���·� ����)
        yield return new WaitForSeconds(1f);
        bossRigid.gravityScale = 1;
        // ���� ���� ���ϸ� ������ �ö󰡼� ������ ������
        // ���߿� ��ȹ�� �ǵ�� �ް� �ǵ��� �´��� QnA
    }

    private IEnumerator SponFireBall()
    {
        // ���濡 ȭ���� �߻� ����

        // ȭ���� �߻� �ִϸ��̼�
        animator.Play("boss1 2 SponFireBall");
        FireBallFire();
        // �ִϸ��̼� ����ð�
        yield return new WaitForSeconds(2.3f);

        // ȭ���� ������ ����
        
        // ȭ������ �����Ǵ� 1.3 �ʵ��� �� ��Ÿ����� ������ ���� �ڷ� �̵��ϸ� ȭ������ �����ڷ� �̵��ϴ� ���� �߻�
        // �÷��̾ 1.3�� ���� �� �Ÿ����� �����ڷ� ���� �Ұ��� �ϴٰ� �Ǵ�. �� ������ �˾Ƶα⸸ ����

        // �� �߻� ���� ���� ��� (1.5�� �� Idle ���·� ����)


    }
    private IEnumerator FireBarrier()
    {
        float bossJumpPower = 20f;

        animator.Play("boss1 2 FireBarrier");
        yield return new WaitForSeconds(1f);

        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        // ������ ���� �ö�
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        // �ö󰡰� ��¡�ϴ� �ִϸ��̼�
        animator.Play("boss1 2 FireBarrier");
        yield return new WaitForSeconds(0.7f);
        // ������ ��ġ ����
        bossRigid.velocity = Vector2.zero;
        bossRigid.bodyType = RigidbodyType2D.Kinematic;

        // �ִϸ��̼� �� �ö󰡴� �ð��� ���� ���ð�
             
        // ������ ������ ȭ�� �庮 ����
        FireWallInstant();

        // �ұ�� ���� ���� ���� ���
        yield return new WaitForSeconds(1.5f);

        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.gravityScale = 5;
        yield return new WaitForSeconds(1f);
        bossRigid.gravityScale = 1;
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
        if (player.transform.position.x < bossObject.transform.position.x)
        {
            // ������ ������ �ٶ󺸵��� ��
            GameObject fireBall = Instantiate(fireBallLeftPre, fireBallPoint.position, fireBallPoint.rotation);
        }
        else
        {
            // ������ �������� �ٶ󺸵��� ��
            GameObject fireBall = Instantiate(fireBallRightPre, fireBallPoint.position, fireBallPoint.rotation);
        }
        
    }

    public void FireWallInstant()
    {
        GameObject fireWall = Instantiate(fireWallPre, fireWallPoint.position, fireWallPoint.rotation);
    }

    public void SlashEffect()
    {
        GameObject fireWall = Instantiate(slash, fireBallPoint.position, fireBallPoint.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� �浹�ߴ��� Ȯ�� Test => wall�� ��ü
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            isWall = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            isWall = false;
        }
    }
}

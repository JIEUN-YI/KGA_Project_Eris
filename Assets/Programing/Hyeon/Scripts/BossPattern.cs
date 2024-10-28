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
        bossPatternNum = 3;

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
                yield return StartCoroutine(FireBarrier());
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
        isWall = false;
        // �÷��̾� ��ġ Ž��
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        // ���� ���� ��ġ
        Vector2 startPosition = transform.position;

        // �ִϸ��̼� ���
        animator.Play("boss1_2_idel");

        // ���� ��ǥ �Ÿ� ����
        float targetDistance = 40f; // ������ �̵��� �Ÿ� (���ϴ� ������ ����)
        float currentDistance = 0f; // ���� �̵��Ÿ�
        float tackleSpeed = 80f;  // ���� �ӵ�

        // ���� ����
        Debug.Log("���� ����---!");

        // while������ ���� �Ÿ��� ����
        while (currentDistance < targetDistance && !isWall)
        {
            // �浹 �����ϰ� ��ǥ ��ġ�� �̵�
            bossRigid.MovePosition(bossRigid.position + playerDirection * tackleSpeed * Time.deltaTime);
            currentDistance = Vector2.Distance(transform.position, startPosition);
            /*
            if (currentDistance >= targetDistance || isWall == true)
            {
                
                break;
            }
            */
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

        Debug.Log("�ҽ��---!");

        // �� �߻� ���� ���� ��� (1.5�� �� Idle ���·� ����)
        yield return new WaitForSeconds(2f);

        Debug.Log("�� �߻� ��---!");
    }
    private IEnumerator FireBarrier()
    {
        float bossJumpPower = 20f;
        // �ö󰡰� ��¡�ϴ� �ִϸ��̼�
        animator.Play("boss1_2_idel");
        // ������ ���� �ö�
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        // ������ ��ġ ����
        //bossRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        bossRigid.bodyType = RigidbodyType2D.Kinematic;
        // �ִϸ��̼� �� �ö󰡴� �ð��� ���� ���ð�
        yield return new WaitForSeconds(1f);
        
      
        // ������ ������ ȭ�� �庮 ����
        FireWallInstant();
        Debug.Log("�ұ�� �����---!");

        // �ұ�� ���� ���� ���� ���
        yield return new WaitForSeconds(2f);
        //bossRigid.constraints = RigidbodyConstraints2D.None;
        //bossRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
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

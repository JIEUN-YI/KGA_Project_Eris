using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾ ���� �� �ִ� ����
    enum PlayerState {Idle, Run, Jump, Fall, Grab, Dash, Attack, Die}; 
    [SerializeField] PlayerState curState;     // �÷��̾��� ���� ����

    private Rigidbody2D rb;

    [SerializeField] Collision coll;

    //private SpriteRenderer spriteRenderer;


    [Header("PlayerInfo")]
    [SerializeField] float maxSpeed = 10f;      // �ִ� �̵� �ӵ�
    [SerializeField] float maxFallSpeed = 10f;  // �ִ� ���� �ӵ�
    [SerializeField] float moveAccel = 30f;     // �̵� ���ӵ�
    [SerializeField] float jumpSpeed = 15f;     // ���� �ӵ�
    [SerializeField] bool canMove = true;       // �̵� ���� ����(���Ͽ�)

    [Header("PlayerStat")]
    [SerializeField] float attackPoint;
    [SerializeField] float curHp;
    [SerializeField] float maxHp;

    [Header("DashInfo")]
    [SerializeField] float dashSpeed = 25f;     // ��� �ӵ�
    [SerializeField] float dashTime = 0.2f;     // ��� ���� �ð�
    private float dashTimeLeft;                 // ��� ���� �ð�
    [SerializeField] bool isDashing = false;    // ��� ������ ����
    [SerializeField] bool canDash = true;       // ��� ���� ����

    [Header("GrapInfo")]
    [SerializeField] float SlipSpeed = 1f;      //���� ����� ���� �� �������� �ӵ�


    [SerializeField] Animator playerAnimator;  
    private int curAniHash;                     //���� ������ �ִϸ��̼��� �ؽ��� ��� ����

    //�÷��̾� �ִϸ��̼��� �Ķ���� �ؽ� ����
    private static int idleHash = Animator.StringToHash("Idle");
    private static int runHash = Animator.StringToHash("Run");
    private static int jumpHash = Animator.StringToHash("Jump");
    private static int fallHash = Animator.StringToHash("Fall");
    private static int grabHash = Animator.StringToHash("Grab");
    private static int attackHash = Animator.StringToHash("Attack");
    private static int dieHash = Animator.StringToHash("Die");

    //�ڿ������� ȸ���� ���� �÷��̾��� �������� �ٲ� ���ӿ�����Ʈ
    [SerializeField] private GameObject gameObject;

    [SerializeField] bool isAttack = false;
    [SerializeField] private Collider2D attackSpot;          //������ ����� ��
    [SerializeField] private GameObject attackEffectPrefabs; //���� ����Ʈ

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if (!canMove) return;

        //���¿� ���� ������Ʈ �Լ� ȣ��
        switch (curState)
        {
            case PlayerState.Idle:
                IdleUpdate();
                break;
            case PlayerState.Run:
                RunUpdate();
                break;
            case PlayerState.Jump:
                JumpUpdate();
                break;
            case PlayerState.Fall:
                FallUpdate();
                break;
            case PlayerState.Grab:
                GrabUpdate();
                break;
            case PlayerState.Dash:
                DashUpdate();
                break;
            case PlayerState.Attack:
                AttackUpdate();
                break;
            case PlayerState.Die:
                DieUpdate();
                break;
        }
    }

    private void FixedUpdate()
    {
        AnimatorPlay();
    }

    private void IdleUpdate()
    {
        Move();

        //�¿� �Է°��� ���� ��
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            curState = PlayerState.Run;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.V) && !isAttack)
        {
            StartCoroutine(Attack());        // ���� �ڷ�ƾ ȣ��
        }
    }

    private void RunUpdate()
    {
        Move();

        //�÷��̾��� �ӵ��� ���� 0�� ��
        if (rb.velocity.sqrMagnitude < 0.01f)
        {
            curState = PlayerState.Idle;
        }
        if (rb.velocity.y < -0.01f)
        {
            curState = PlayerState.Fall;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.X) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.V) && !isAttack)
        {
            StartCoroutine(Attack());        // ���� �ڷ�ƾ ȣ��
        }
    }

    private void JumpUpdate()
    {
        Move();

        //���� �پ� ������ y�� �ӵ��� ��ȭ�� ���� ���� ��
        if (coll.onGround && rb.velocity.y < 0.01f)
        {
            curState = PlayerState.Idle;
            canDash = true;
        }
        else if (rb.velocity.y < -0.01f)
        {
            curState = PlayerState.Fall;  // ���� ���·� ��ȯ
        }

        if (Input.GetKeyDown(KeyCode.Z) && coll.onWall)
        {
            Grab();
        }

        if (Input.GetKeyDown(KeyCode.C) && coll.onWall)
        {
            GrabJump();
        }

        if (Input.GetKeyDown(KeyCode.X) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.V) && !isAttack)
        {
            StartCoroutine(Attack());        // ���� �ڷ�ƾ ȣ��
        }
    }

    private void FallUpdate()
    {
        Move();

        // �����ϸ� Idle ���·� ��ȯ
        if (coll.onGround)
        {
            curState = PlayerState.Idle;
            canDash = true;
        }

        if (Input.GetKeyDown(KeyCode.Z) && coll.onWall)
        {
            Grab();
        }

        if (Input.GetKeyDown(KeyCode.C) && coll.onWall)
        {
            GrabJump();
        }
        if (Input.GetKeyDown(KeyCode.X) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.V) && !isAttack)
        {
            StartCoroutine(Attack());        // ���� �ڷ�ƾ ȣ��
        }
    }

    private void GrabUpdate()
    {
        //GrabMove();

        if (Input.GetKeyUp(KeyCode.Z))
        {
            UnGrab();
        }

        if (!coll.onWall) // ���� �پ����� ���� ��
        {
            rb.gravityScale = 0f; // �߷� ��Ȱ��ȭ
        }
        else
        {
            rb.gravityScale = 1f; // �߷� Ȱ��ȭ
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -SlipSpeed)); // ������ ������
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GrabJump();
        }
    }

    private void DashUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && coll.onWall)
        {
            Grab();
        }

        if (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            if (coll.onGround)
            {
                curState = PlayerState.Idle;
            }
            curState = PlayerState.Fall;  // ��� ���� �� ���� ���·� ��ȯ
        }
    }

    private void AttackUpdate()
    {

    }

    private void DieUpdate()
    {

    }

    private void Move()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        /*if (xInput < 0)
        {
            spriteRenderer.flipX = true; // ���� �������� �̵� �� ��������Ʈ�� ������
        }
        else if (xInput > 0)
        {
            spriteRenderer.flipX = false; // ������ �������� �̵� �� ��������Ʈ�� ��������
        }*/
        if (xInput > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (xInput < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //float xSpeed = Mathf.Lerp(rb.velocity.x, xInput * maxSpeed, moveAccel);
        float xSpeed = Mathf.MoveTowards(rb.velocity.x, xInput * maxSpeed, Time.deltaTime * moveAccel);
        float ySpeed = Mathf.Max(rb.velocity.y, -maxFallSpeed);

        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    private void Jump()
    {
        curState = PlayerState.Jump;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    private void Grab()
    {
        if (!coll.onWall)
            return;

        curState = PlayerState.Grab;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    private void UnGrab()
    {
        curState = PlayerState.Jump;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;
    }

    /*private void GrabMove()
    {
        rb.velocity = Vector2.up * Input.GetAxisRaw("Vertical") * 3f;
    }*/

    private void GrabJump()
    {
        curState = PlayerState.Jump;
        rb.gravityScale = 1f;
        
        if (coll.onLeftWall)
        {
            rb.velocity = new Vector2(13f, 10f);
            //������ ���� ���ʺ��� �� �������� �⺻������ �������� ������
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        else if (coll.onRightWall)
        {
            rb.velocity = new Vector2(-13f, 10f);
            //������ ���� �����ʺ��� �� �������� �ݴ������ ������ ������
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Dash()
    {
        isDashing = true;
        curState = PlayerState.Dash;
        dashTimeLeft = dashTime;

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        //��� ���� ����
        Vector2 dashDirection = new Vector2(xInput, yInput).normalized;

        if (dashDirection == Vector2.zero)
        {
            // �÷��̾��� ȸ�� ���� ���� ��� ���� ����
            if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0f)) // �������� �ٶ� ��
            {
                dashDirection = Vector2.right; // ���������� ���
            }
            else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180f)) // ������ �ٶ� ��
            {
                dashDirection = Vector2.left; // �������� ���
            }
        }

        rb.velocity = dashDirection * dashSpeed;
        canDash = false;
    }

    private IEnumerator Attack()
    {
        isAttack = true;
        curState = PlayerState.Attack;  // ���� ���·� ��ȯ

        // ���� ����Ʈ ����
        GameObject attackEffect = Instantiate(attackEffectPrefabs, transform.position, Quaternion.identity);

        // 1�� ���
        yield return new WaitForSeconds(1f);

        // ����Ʈ �� �ݶ��̴� ����
        Destroy(attackEffect);

        // ���� ���¸� Idle�� ��ȯ
        curState = PlayerState.Idle;

        isAttack = false;
    }

    public void StopMovement()
    {
        // �÷��̾��� �ӵ��� 0���� �����Ͽ� �������� ����
        rb.velocity = Vector2.zero;

        // �߷� �������� 0���� �����Ͽ� �߷� ������ ���� �ʵ��� ��
        rb.gravityScale = 0f;

        // �÷��̾��� ���� ���¸� Idle�� ��ȯ�Ͽ� ������ ���� �ʱ�ȭ
        curState = PlayerState.Idle;
    }

    private void AnimatorPlay()
    {
        int temp = idleHash;
        if (curState == PlayerState.Idle)
        {
            temp = idleHash;
        }
        if (curState == PlayerState.Run)
        {
            temp = runHash;
        }    
        if (curState == PlayerState.Jump)
        {
            temp = jumpHash;
        }
        /*if (curState == PlayerState.Fall)
        {
            temp = fallHash;
        }
        if (curState == PlayerState.Grab)
        {
            temp = grabHash;
        }*/

        if (curAniHash != temp)
        {
            curAniHash = temp;
            //playerAnimator.Play(curAniHash);

            //�ִϸ��̼��� ���� �ؽð��� ���� �÷����ϸ� ��ȯ�ð��� 0.1�ʷ� �ΰ� �⺻ ���̾��� �ִϸ��̼��� ��ȯ�Ѵ�.
            playerAnimator.CrossFade(curAniHash, 0.1f, 0);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss")) // ������ �浹 ��
        {
            StartCoroutine(StunTime(1f));            // 1�� ���� �Է� ��Ȱ��ȭ
        }
    }

    private IEnumerator StunTime(float duration)
    {
        canMove = false;          // �Է� ��Ȱ��ȭ
        playerAnimator.speed = 0; // ��� �ִϸ��̼� �Ͻ� ����

        yield return new WaitForSeconds(duration); // 1�� ���

        canMove = true;           // �Է� �ٽ� Ȱ��ȭ
        playerAnimator.speed = 1; // �ִϸ��̼� ��Ȱ��ȭ
    }
}

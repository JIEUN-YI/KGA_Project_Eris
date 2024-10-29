using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //�ڿ������� ȸ���� ���� �÷��̾��� �������� �ٲ� ���ӿ�����Ʈ
    [SerializeField] private GameObject gameObjectRotationPoint;

    //�÷��̾ ���� �� �ִ� ����
    public enum PlayerState {Idle, Run, Jump, Fall, Grab, Dash, Attack, Die}; 
    [SerializeField] public PlayerState curState;     // �÷��̾��� ���� ����

    private Rigidbody2D rb;

    [SerializeField] Collision coll;

    [Header("MoveInfo")]
    [SerializeField] float maxSpeed = 10f;      // �ִ� �̵� �ӵ� 
    [SerializeField] float moveAccel = 30f;     // �̵� ���ӵ�
    [SerializeField] bool canMove = true;       // �̵� ���� ����(���Ͽ�)
    [SerializeField] float TestSpeed;           // ĳ���� ���ν�Ƽ ��ȭ��(�׽�Ʈ��)

    [Header("Jump&Fall")]
    [SerializeField] float jumpSpeed = 15f;     // ���� �ӵ�
    [SerializeField] float maxFallSpeed = 10f;  // �ִ� ���� �ӵ�
    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    private Vector2 reducedColliderSize;

    [Header("DashInfo")]
    [SerializeField] float dashSpeed = 25f;     // ��� �ӵ�
    [SerializeField] float dashTime = 0.2f;     // ��� ���� �ð�
    private float dashTimeLeft;                 // ��� ���� �ð�
    [SerializeField] bool isDashing = false;    // ��� ������ ����
    [SerializeField] bool canDash = true;       // ��� ���� ����

    [Header("GrapInfo")]
    [SerializeField] float SlipSpeed = 1f;      // ���� ����� ���� �� �������� �ӵ�

    [Header("AnamationInfo")]
    [SerializeField] Animator playerAnimator;  
    private int curAniHash;                     // ���� ������ �ִϸ��̼��� �ؽ��� ��� ����
    [SerializeField] GameObject GFX;            // ĳ���� ȸ���� ���� �θ� ������Ʈ

    //�÷��̾� �ִϸ��̼��� �Ķ���� �ؽ� ����
    private static int idleHash = Animator.StringToHash("Idle");
    private static int runHash = Animator.StringToHash("Run");
    private static int jumpHash = Animator.StringToHash("Jump");
    private static int fallHash = Animator.StringToHash("Fall");
    private static int grabHash = Animator.StringToHash("Grab");
    private static int attack1Hash = Animator.StringToHash("Attack1");
    private static int attack2Hash = Animator.StringToHash("Attack2");
    private static int attack3Hash = Animator.StringToHash("Attack3");
    private static int dieHash = Animator.StringToHash("Die");

    [Header("AttackInfo")]
    [SerializeField] bool isAttack = false;
    //[SerializeField] private Collider2D attackSpot;          //������ ����� ��
    [SerializeField] private GameObject attackEffectPrefabs;   //���� ����Ʈ
    [SerializeField] AttackTest attackTest;                    //���� ���� ����       
    [SerializeField] private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = boxCollider.size;
        reducedColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f);

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
        TestSpeed = rb.velocity.sqrMagnitude;
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
        if (rb.velocity.sqrMagnitude < 0.01f )
        {
            curState = PlayerState.Idle;
        }
        if (rb.velocity.y < -0.01f && !coll.onGround)
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
        else if (rb.velocity.y < -0.01f && !coll.onGround)
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
            GFX.transform.localScale = new Vector3(1, 1 ,1);
        }
        else if (xInput < 0)
        {
            GFX.transform.localScale = new Vector3(-1, 1, 1);
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
            GFX.transform.localScale = new Vector3(1, 1, 1);
        }
        
        else if (coll.onRightWall)
        {
            rb.velocity = new Vector2(-13f, 10f);
            //������ ���� �����ʺ��� �� �������� �ݴ������ ������ ������
            GFX.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Dash()
    {
        isDashing = true;
        curState = PlayerState.Dash;
        dashTimeLeft = dashTime;

        float xInput = Input.GetAxisRaw("Horizontal");

        // ��� ���� ����
        Vector2 dashDirection;

        if (xInput != 0)
        {
            // ���� �Է��� ���� ��, �ش� �������� ���
            dashDirection = new Vector2(xInput, 0).normalized;
        }
        else
        {
            // ���� �Է��� ���� �� �÷��̾��� ȸ�� ���� ���� ��� ���� ����
            if (GFX.transform.localScale.x == 1f) // �������� �ٶ� ��
            {
                dashDirection = Vector2.right; // ���������� ���
            }
            else if (GFX.transform.localScale.x == -1f) // ������ �ٶ� ��
            {
                dashDirection = Vector2.left; // �������� ���
            }
            else
            {
                dashDirection = Vector2.zero; // ��� ������ �������� ���� ���
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
        Vector2 effectPosition = attackTest.attackRangeCollider.transform.position;
        
        // ���� ����Ʈ ���� ����
        Quaternion effectRotation = Quaternion.identity;

        //���⿡ ���� ����Ʈ ȸ��
        if (GFX.transform.localScale.x == 1f) // �������� �ٶ� ��
        {
            effectRotation = Quaternion.identity; // �⺻ ȸ�� ����
        }
        else if (GFX.transform.localScale.x == -1f) // ������ �ٶ� ��
        {
            effectRotation = Quaternion.Euler(0f, 0f, 180f); // z�� ���� 180�� ȸ��
        }

        // ����Ʈ ����
        GameObject attackEffect = Instantiate(attackEffectPrefabs, effectPosition, effectRotation);

        if (attackTest.IsBossInRange)
        {
            Debug.Log("�������� ���ظ� ����");
        }

        // @�� ���
        yield return new WaitForSeconds(0.5f);

        // ����Ʈ �� �ݶ��̴� ����
        Destroy(attackEffect);

        // ���� ���¸� Idle�� ��ȯ
        curState = PlayerState.Idle;

        isAttack = false;
    }

    public void Die()
    {
        isDead = true;
        //curState = PlayerState.Die;
        Debug.Log("���");
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
        if (curState == PlayerState.Attack)
        {
            temp = attack1Hash;
        }


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

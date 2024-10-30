using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public event EventHandler OnJumpDown;

    [Header("DashInfo")]
    [SerializeField] float dashSpeed = 25f;     // ��� �ӵ�
    [SerializeField] float dashTime = 0.4f;     // ��� ���� �ð�
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
    private static int dashHash = Animator.StringToHash("Dash");
    private static int landingHash = Animator.StringToHash("Landing");
    private static int dieHash = Animator.StringToHash("Die");

    [Header("AttackInfo")]
    [SerializeField] bool isAttack = false;
    //[SerializeField] private Collider2D attackSpot;          //������ ����� ��
    [SerializeField] private GameObject attackEffectPrefabs;   //���� ����Ʈ
    [SerializeField] AttackTest attackTest;                    //���� ���� ����       
    [SerializeField] private bool isDead = false;              //�÷��̾��� ���� �Ǻ�
    [SerializeField] private int currentAttackCount = 0;       //���� ���� Ƚ��
    private float lastAttackTime;                              //������ ���� �ð�
    public float comboResetTime = 1.5f;                        //���� �޺��� �ʱ�ȭ �Ǵ� �ð�


    //[Header("CameraInfo")]
    //[SerializeField] CameraController CameraController;


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

        ComboUpdate(); //������ �ð� ���� ������ �̷������ ������ �����޺� �ʱ�ȭ


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

        //�÷��� ���̾ �ؿ� ���� �� ���� �߰�
        /*if (Input.GetKey(KeyCode.DownArrow) &&
            Input.GetKeyDown(KeyCode.C))
        {
            LowJump();
        }*/
        if (Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.X) && !isAttack)
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
        
        /*if (Input.GetKey(KeyCode.DownArrow) &&
            Input.GetKeyDown(KeyCode.C))
        {
            LowJump();
        }*/

        if (Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.X) && !isAttack)
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

        if ((coll.onLeftWall && Input.GetKey(KeyCode.LeftArrow)) || (coll.onRightWall && Input.GetKey(KeyCode.RightArrow)))
        {
            Grab();
        }

        if (Input.GetKeyDown(KeyCode.C) && coll.onWall)
        {
            GrabJump();
        }

        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.X) && !isAttack)
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

        if ((coll.onLeftWall && Input.GetKey(KeyCode.LeftArrow)) || (coll.onRightWall && Input.GetKey(KeyCode.RightArrow)))
        {
            Grab();
        }

        if (Input.GetKeyDown(KeyCode.C) && coll.onWall)
        {
            GrabJump();
        }
        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.X) && !isAttack)
        {
            StartCoroutine(Attack());        // ���� �ڷ�ƾ ȣ��
        }
    }

    private void GrabUpdate()
    {
        if ((coll.onLeftWall && Input.GetKey(KeyCode.LeftArrow)) || (coll.onRightWall && Input.GetKey(KeyCode.RightArrow)))
        {
            rb.gravityScale = 0f; // �߷� ��Ȱ��ȭ
            rb.velocity = Vector2.zero; // �ӵ��� 0���� �����Ͽ� ���� ����
        }
        else
        {
            UnGrab(); // ����Ű�� ���� ����� ���� ����
            return;
        }

        // ����� ���¿��� ���� �Է� �� ������ ����
        if (Input.GetKeyDown(KeyCode.C))
        {
            GrabJump();
        }
    }

    private void DashUpdate()
    {
        if ((coll.onLeftWall && Input.GetKey(KeyCode.LeftArrow)) || (coll.onRightWall && Input.GetKey(KeyCode.RightArrow)))
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

    private void ComboUpdate()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            currentAttackCount = 0;
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
            //CameraController.isLeft = false;
        }
        else if (xInput < 0)
        {
            GFX.transform.localScale = new Vector3(-1, 1, 1);
            //CameraController.isLeft = true;
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

    /*private void LowJump()
    {
        curState = PlayerState.Fall;
        OnJumpDown?.Invoke(this, EventArgs.Empty);
    }*/

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
        curState = PlayerState.Fall;
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
            rb.velocity = new Vector2(40f, 25f);
            //������ ���� ���ʺ��� �� �������� �⺻������ �������� ������
            GFX.transform.localScale = new Vector3(1, 1, 1);
        }
        
        else if (coll.onRightWall)
        {
            rb.velocity = new Vector2(-40f, 25f);
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

        currentAttackCount++;
        lastAttackTime = Time.time;

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

        isAttack = false;

        if(currentAttackCount >= 3)
        {
            currentAttackCount = 0;
        }

        // ���� ���¸� Idle�� ��ȯ
        curState = PlayerState.Idle;
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
        if (curState == PlayerState.Fall)
        {
            temp = fallHash;
        }


        if (curState == PlayerState.Grab)
        {
            temp = grabHash;
        }
        if (curState == PlayerState.Dash)
        {
            temp = dashHash;
        }


        if (curState == PlayerState.Attack)
        {
            switch (currentAttackCount)
            {
                case 1:
                    temp = attack1Hash;
                    break;
                case 2:
                    temp = attack2Hash;
                    break;
                case 3:
                    temp = attack3Hash;
                    break;
            }        
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

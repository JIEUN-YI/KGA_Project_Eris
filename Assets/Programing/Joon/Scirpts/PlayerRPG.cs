using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRPG : MonoBehaviour
{
    [Header("PlayerStat")]
    [SerializeField] public float attackDamage;
    [SerializeField] public float curHp;
    [SerializeField] public float maxHp;

    private PlayerController playerController;

    private void Awake()
    {
        curHp = maxHp;
        // PlayerController ������Ʈ ����
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        //�׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1f);
        }
    }

    public void Dealdamage(float damage)
    {

    }

    public void TakeDamage(float damage)
    {
        curHp -= damage;
        Debug.Log($"���� ü�� : {curHp}");

        // �÷��̾��� ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (curHp <= 0)
        {
            playerController.Die();
        }
    }
}

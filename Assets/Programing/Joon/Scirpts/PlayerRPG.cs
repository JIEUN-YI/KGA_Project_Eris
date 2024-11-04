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
    private Collision coll;

    private void Awake()
    {
        curHp = maxHp;
        // PlayerController ������Ʈ ����
        playerController = GetComponent<PlayerController>();
        coll = GetComponent<Collision>();
    }

    private void Update()
    {
        StampedeUpdate();
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

    public void StampedeUpdate()
    {
        if(coll.onPlatform && coll.onCeiling)
        {
            playerController.Die();
        }
    }
}

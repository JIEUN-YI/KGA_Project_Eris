using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerRPG : MonoBehaviour
{
    [Header("PlayerStat")]
    [SerializeField] public float attackDamage;
    [SerializeField] public float curHp;
    [SerializeField] public float maxHp;

    [Header("BossSort")]
   // [SerializeField] Boss02 Boss1;
    [SerializeField] Boss1Phase1 Boss2_1;
    [SerializeField] BossPattern Boss2_2;
    [SerializeField] Boss3Controller Boss3;

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
    }

    public void DealDamageToBoss(string bossType, float damage)
    {
        switch (bossType)
        {
            case "Boss1":
               // Boss1.TakeDamage(damage);
                break;
            case "Boss2_1":
                Boss2_1.TakeDamage(damage);
                break;
            case "Boss2_2":
                Boss2_2.TakeDamage(damage);
                break;
            case "Boss3_1":
                Boss3.TakeDamage(damage);
                break;
            case "Boss3_2":
                Boss3.TakeDamage(damage);
                break;
            default:
                Debug.LogWarning("�������� �ʴ� �����Դϴ�.");
                break;
        }
        Debug.Log($"{damage}�� ���ظ� �������ϴ�.");
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

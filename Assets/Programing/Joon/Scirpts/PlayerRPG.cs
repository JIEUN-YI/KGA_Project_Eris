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

    private void Start()
    {
        StartHealthDecrease();
    }

    public void TakeDamage(float damage)
    {
        curHp -= damage;

        // �÷��̾��� ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (curHp <= 0)
        {
            playerController.Die();
        }
    }

    public IEnumerator DecreaseHealthOverTime()
    {
        while (curHp > 0)
        {
            yield return new WaitForSeconds(1f); // 1�� ���
            TakeDamage(1); // ���� 1�� ���ظ� �ݴϴ�.
        }
    }

    // �� �޼��带 ȣ���Ͽ� �ڷ�ƾ�� ������ �� �ֽ��ϴ�.
    public void StartHealthDecrease()
    {
        StartCoroutine(DecreaseHealthOverTime());
    }
}

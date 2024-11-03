using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab; // �߻��� �Ѿ��� ������
    [SerializeField] Transform bulletSpawnPoint; // �Ѿ��� �߻� ��ġ
    [SerializeField] float bulletSpeed = 10f; // �Ѿ��� �ӵ�
    [SerializeField] float fireGap = 0.1f; // ���� �߻� ����
    [SerializeField] bool isFiring = false; // �߻� ����

    [SerializeField] Animator DronAnimator;
    //private static int dronIdleHash = Animator.StringToHash("DroneIdle");
    //private static int dronAttackHash = Animator.StringToHash("DroneAttack");
    [SerializeField] Coroutine FireCoroutine;
    [SerializeField] GameObject GFX;
    private void Awake()
    {
        // ���� GameObject�� �ִ� Animator�� �ڵ����� �Ҵ�
        if (DronAnimator == null)
        {
            DronAnimator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        // V Ű �Է����� �߻� �¿���
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFiring = !isFiring;
            if (isFiring)
            {
                FireCoroutine = StartCoroutine(FireBullets());
                DronAnimator.SetBool("IsAttacking", true); // ���� �ִϸ��̼� ����
            }
            else
            {
                if (FireCoroutine != null)
                {
                    StopCoroutine(FireCoroutine);
                }
                DronAnimator.SetBool("IsAttacking", false); // Idle �ִϸ��̼����� ���ư�
            }
        }
    }

    // �Ѿ� �߻� �ڷ�ƾ
    private IEnumerator FireBullets()
    {

        while (isFiring)
        {
            for (int i = 0; i < 3; i++)
            {
                FireSingleBullet();
                yield return new WaitForSeconds(fireGap); // �߻� ����
            }
            yield return new WaitForSeconds(1f); // �� �� �߻� �� ��߻� ��� �ð�
        }
    }

    // ���� �Ѿ� �߻�
    private void FireSingleBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        BulletController bulletScript = bullet.GetComponent<BulletController>();
        
        Vector2 bulletdirection = Vector2.zero;   //�Ѿ� ������ ����Ű�� ����(�ʱ�ȭ)
        if (bulletScript != null)
        {
            if (GFX.transform.localScale.x == 1f) // �������� �ٶ� ��
            {
                bulletdirection = Vector2.right;
            }
            else if (GFX.transform.localScale.x == -1f) // ������ �ٶ� ��
            {
                bulletdirection = Vector2.left;
            }
            bulletScript.SetSpeed(bulletdirection * bulletSpeed); // �Ѿ� �ӵ� ����
        }
    }
}

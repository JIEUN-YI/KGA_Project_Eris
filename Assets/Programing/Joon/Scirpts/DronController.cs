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
                DronAnimator.Play("DroneAttack");//�ӽ� ���� �ʿ�
            }
            else
            {
                StopCoroutine(FireBullets());
                DronAnimator.Play("DronIdle");
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
        if (bulletScript != null)
        {
            Vector2 direction = bulletSpawnPoint.right; // ������ ����
            bulletScript.SetSpeed(direction * bulletSpeed); // �Ѿ� �ӵ� ����
        }
    }
}

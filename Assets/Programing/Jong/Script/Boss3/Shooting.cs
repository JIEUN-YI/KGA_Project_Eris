using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Transform[] shootingPoints;

    [SerializeField] GameObject player;


    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject bulletPrefab;



    private void Update()
    {
        
    }

    IEnumerator ShootingBullet() //�Ѿ� ���� 
    {
        while (true) 
        {
            yield return null;
        }

        
    }


    IEnumerator ShootingLaser() //������ ����
    {
        for (int i = 0; i < 6; i++) 
        {
               
        }

        yield return null;
    }

    IEnumerator ShootingMissile() // �̻��� ���� 
    {
        for (int i = 0; i < 3; i++) 
        {
           int x = Random.Range(0, 7);
        }
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAura : MonoBehaviour
{
    // �÷��̾� ������
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainPre;
    // �˱��� ���ǵ�
    [SerializeField] float swordAuraSpeed;

    // �߻� ����
    public int direction;
    
    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        Destroy(gameObject, 3f);
    }   

    private void Update()
    {
        mainPre.transform.Translate(transform.forward * direction * swordAuraSpeed * Time.deltaTime);
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // �Ҹ� �ִϸ��̼� ���� ����
            Destroy(mainPre);
        }
    }
}

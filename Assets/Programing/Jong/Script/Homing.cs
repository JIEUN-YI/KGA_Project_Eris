using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] GameObject main;
    [SerializeField] GameObject exEffect;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        HomingMissile();
    }


    private void HomingMissile()  // 아무 오브젝트에 붙이고 플레이어 게임 오브젝트를 불러오면 유도탄이 됨 
    {
        Vector2 direction = (Vector2)player.transform.position - (Vector2)transform.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z; // 이해안감 ; 아무튼 벡터의 외적 구해서 그 방향으로 회전 
        transform.Rotate(0, 0, -rotateAmount * 100f * Time.deltaTime); // 프레임 마다 플레이어 방향으로 각도를 틀고 (y축을 플레이어로 향하게) 이동 
                                                                       // 유도 성능을 낮추려면 이동속도, 각 회전속도 , 함수의 호출횟수?(프레임 마다가 아닌 코루틴으로 몇초마다 이런식으로 조절)를 조정해야함
        transform.Translate(Vector2.up * 1f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {   
            capsuleCollider.enabled = false;    
            main.SetActive(false);
            exEffect.SetActive(true);
            Destroy(gameObject, 0.4f);
        }
    }
}

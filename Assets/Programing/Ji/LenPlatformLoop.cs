using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenPlatformLoop : MonoBehaviour
{
    [SerializeField] MoveLenPlatform moveLenPlatform;
    [SerializeField] bool isUpMove;
    [SerializeField] GameObject RiseCreatePos;
    [SerializeField] GameObject DesCreatePos;

    // Respawn �±׿� �浹�ϴ� ��� ������Ʈ�� ��ġ�� ���ġ
    // ��, MoveLenPlatform.cs���� IsUpMove�� �޾Ƽ�
    // IsUpMove == true (�ö󰡴� ��) �϶��� UpCreatePos�� ���ġ
    // IsUpMove == false (�������� ��) �϶��� DownCreatePos�� ���ġ

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (moveLenPlatform.isUpMove == true)
            {
                gameObject.transform.position = new Vector2(RiseCreatePos.transform.position.x, RiseCreatePos.transform.position.y);
            }
            else if(moveLenPlatform.isUpMove == false)
            {
                gameObject.transform.position = new Vector2(DesCreatePos.transform.position.x, DesCreatePos.transform.position.y);
            }

        }
    }

}


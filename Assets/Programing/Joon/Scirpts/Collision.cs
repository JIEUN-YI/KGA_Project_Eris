using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Collision")]

    [SerializeField] float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugColor = Color.red;


    [Header("Collision Settings")]

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public bool onGround;
    [SerializeField] public bool onWall;
    [SerializeField] public bool onRightWall;
    [SerializeField] public bool onLeftWall;
    [SerializeField] public int wallSide;

    private void Update()
    {
        //������ ����(collisionRadius)�� �����浹ü�� �����Ͽ� �׶��� ���̾ ��Ҵ��� �Ǻ�
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        //�����ʺ��̳� ���ʺ� �� �� �ϳ��� ��Ƶ� ���� ���� ������ �Ǻ�
        onWall = onRightWall || onLeftWall;

        //������ ���� ������� ��� -1 / ���� ���� ������� ��� 1 / ��� ���� ����ִ��� �Ǻ��ϴ� ����
        wallSide = onRightWall ? -1 : 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        //�浹 ���� Ȯ�ο� ����� ǥ��
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}

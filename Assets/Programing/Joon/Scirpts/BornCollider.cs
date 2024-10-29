using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornCollider : MonoBehaviour
{
    public GameObject[] bones; // ���̷��� ĳ������ �� ����Ʈ�� �迭�� ����
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>(); // ���ٸ� �߰�
        }
    }

    private void Update()
    {
        UpdateColliderBounds();
    }

    private void UpdateColliderBounds()
    {
        if (bones == null || bones.Length == 0)
        {
            return;
        }

        float minX = bones[0].transform.position.x;
        float maxX = bones[0].transform.position.x;
        float minY = bones[0].transform.position.y;
        float maxY = bones[0].transform.position.y;

        // ��� ���� x, y ��ǥ�� ���Ͽ� ������ ã��
        foreach (var bone in bones)
        {
            Vector2 bonePos = bone.transform.position;

            if (bonePos.x < minX) minX = bonePos.x;
            if (bonePos.x > maxX) maxX = bonePos.x;
            if (bonePos.y < minY) minY = bonePos.y;
            if (bonePos.y > maxY) maxY = bonePos.y + 1f;
        }

        // �ݶ��̴��� �߽� ��ġ�� ũ�� ���
        Vector2 center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
        Vector2 size = new Vector2(maxX - minX, maxY - minY);

        boxCollider.offset = transform.InverseTransformPoint(center); // ���� ��ǥ��� ��ȯ
        boxCollider.size = size;
    }
}

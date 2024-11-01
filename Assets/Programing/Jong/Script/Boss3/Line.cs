using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] LineRenderer lineRenderer;

    // �÷��̾ �����ϴ� ������ �ڵ� 
    void Start()
    {
        lineRenderer.positionCount = 2;    
    }

   
    void Update()
    {
        lineRenderer.SetPosition(0, startPoint.position);
        //lineRenderer.startColor = Color.red;
        Vector3 direction = (endPoint.position - startPoint.position).normalized; // ������ ��� 
        Vector3 endPointover = startPoint.position + direction * 500f; // ������ ���� 
        lineRenderer.SetPosition(1, (Vector2)endPointover);  // ���� ������ ���̸�ŭ �� ���� ������ �׸�
    }
}

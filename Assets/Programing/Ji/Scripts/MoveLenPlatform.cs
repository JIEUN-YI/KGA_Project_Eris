using UnityEngine;

public class MoveLenPlatform : MonoBehaviour
{
    // ������ �ӵ���
    [SerializeField] public float moveSpeed;

    // ���⺰�� �̵�
    // PlatformAttackPattern���� ���κ��� �޾ƿ����� �����ϱ�
    // [SerializeField] PatternController patternController;
    [SerializeField] public bool isUpMove;

    private void Start()
    {
        int num = Random.Range(0, 2);
        switch (num)
        {
            case 0:
                isUpMove = false;
                Debug.Log("�ٿ�");
                break;
            case 1:
                isUpMove = true;
                Debug.Log("��");
                break;
        }

    }

    private void Update()
    {
        if (isUpMove)
        {
            MoveUp();
        }
        else if (!isUpMove)
        {
            MoveDown();
        }
    }

    /// <summary>
    /// ������ ������ �ӵ��� ���� ��� �̵�
    /// </summary>
    public void MoveUp()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }

    public void MoveDown()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

}

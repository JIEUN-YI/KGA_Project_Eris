using UnityEngine;

public class DeadZone : MonoBehaviour
{
    /// <summary>
    /// DeadZone�� �浹ü�� �浹�ϴ� ��� ����
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /* tag�� DeadZone�� ��� 
             * �÷��̾��� ���
             * DieUpdate()
             */
            Debug.Log("�÷��̾� ���");
        }
    }
}

using UnityEngine;

public class DeadZone : MonoBehaviour
{
    /// <summary>
    /// DeadZone�� trigger�� ����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
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

using UnityEngine;


/// <summary>
/// ������ ���� ���� �� �ϳ���
/// �������� �� ���ǵ��� �� �Ǵ� �Ʒ��� �����̵��� ����
/// </summary>
public class PatternController : MonoBehaviour
{
    // ������ ���¿� ����
    // ������ ü���� 25% �̻��� ��� �� ���� ���� �� ������ ���� �����̵��� ����
    // ������ ü���� 25% ������ ��� �� ���� ���� �� ������ ���� �����̵��� ����

    [SerializeField] public bool isUpMove; // true �̸� ���� �̵� - �� �������� ���� �� �޾ƿ����� �����ϱ�

    //������ ���� ���¿� ���� UpDate()���� �б�
    public enum BossState { normal, overdirve }
    // ������ ü���� 25% �̻��� ��� - normal
    // ������ ü���� 25% ������ ��� - overdive
    public BossState nowState; // ������ ���� ����

    private void Update()
    {
        switch (nowState)
        {
            case BossState.normal:

                break;
            case BossState.overdirve:

                break;
        }
    }

    public void setNormalChoice()
    {

    }

    public void setOverdirveChoice()
    {

    }





}

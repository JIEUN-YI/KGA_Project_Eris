using TMPro;
using UnityEngine;
/* ��� ��ũ��Ʈ ���� ��Ʃ�� ����
 * https://www.youtube.com/watch?v=DPWvoUlHbjg&list=PLUZ5gNInsv_NG_UKZoua8goQbtseAo8Ow&index=11
 * https://www.youtube.com/watch?v=1fRbGvQlIEQ
 * https://www.youtube.com/watch?v=_04sCWLHoXU
 * https://www.youtube.com/watch?v=qJjfYvEYKiE
 * 
 * ��縦 Ÿ���������� ����ϴ� �� ���� ��Ʃ�� ����
 * https://www.youtube.com/watch?v=OjcPuEVQT6s
 */
public class DialogueManager : MonoBehaviour
{
    // DatabaseManager.cs�� GetDialogues() �Լ��� ����ϱ� ���� ����
    [SerializeField] DatabaseManager databaseManager;
/*
    [Header("UI")]
    [SerializeField] public GameObject imgBoss; // ���� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] public GameObject imgPlayer; // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] public GameObject imgDialogue; // ��ȭâ �̹���

    [SerializeField] public GameObject uiTextName; // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
    [SerializeField] public GameObject uiTextDialogue; // ĳ���Ͱ� ��� ��� ������Ʈ
*/
    [Header("Test")]
    [SerializeField] private TextMeshProUGUI textName; // ��ȭ�ϴ� ĳ������ �̸� text
    [SerializeField] private TextMeshProUGUI textDialogue; // ĳ������ ��� text
    /*
    private void Awake()
    {
        imgBoss.SetActive(false); // ���� ĳ������ ��ȭ �� ��� �̹���
        imgPlayer.SetActive(false); // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
        imgDialogue.SetActive(true); // ��ȭâ �̹���
        uiTextName.SetActive(false); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
        uiTextDialogue.SetActive(false); // ĳ���Ͱ� ��� ��� ������Ʈ
    }*/

    /// <summary>
    /// �̺�Ʈ�� �����ϴ� ��ȣ�� ������ ��ȣ�� ���� Text�� �˸��� ��ġ�� ����ϰ�
    /// �̹����� �ٲٴ� �Լ�
    /// </summary>
    /// <param name="startNum"></param>
    /// <param name="endNum"></param>
    public void NowDialogue(int startNum, int endNum)
    {
        // nowStart�� �����ͺ��̽����� ����� �迭�� ������ �ҷ���
        Dialogue[] nowStart = databaseManager.GetDialogues(startNum, endNum);

        int i = 0; // �迭�� ��������

        // do - while ������ ������ Ȯ���ϸ鼭 �ݺ�
        do // ������ ù��° �迭�� ���
        {
            Debug.Log(nowStart[i].eventName);
            Debug.Log(nowStart[i].name);
            FindNameImage(nowStart, i); // �̸����� �̹��� ������Ʈ Ȱ��ȭ
            int j = 0; // nowStart[i]�� �迭�� contexts[j]�� Ž���Ͽ� ����ϱ� ���� ������ ����
            Debug.Log(nowStart[i].contexts[j]);
            ShowText(nowStart, i, j);
            if (Input.GetMouseButtonDown(0))
            {
                if (++i < nowStart.Length) // �迭�� ���̰� ����� �ʴ� ������
                {
                    while (nowStart[i].name == "") // �̸��� ������ ��쿡�� ���� ĳ���Ͱ� ���� ��縦 �����
                    {
                        j++;
                        Debug.Log(nowStart[i].contexts[j]);
                        ShowText (nowStart, i, j);
                    }
                }
            }
        } while (nowStart[i].eventName != "end"); // nowStart�迭�� �̺�Ʈ�̸����� end�� ������ ���� �� ��� �ݺ����� ����
    }

    private void FindNameImage(Dialogue[] nowStart, int i)
    {
        if (nowStart[i].name == "homunculus")
        {
            textName.text = "ȣ��Ŭ�罺"; // �̸� Text ���
        }
        else if (nowStart[i].name == "Eris")
        {

            textName.text = "������";
        }
        else return;
    }
    private void ShowText(Dialogue[] nowStart, int i, int j)
    {

        textDialogue.text = nowStart[i].contexts[j].ToString();
    }
}

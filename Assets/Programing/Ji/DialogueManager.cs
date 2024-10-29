using System.Collections;
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

// 
public class DialogueManager : MonoBehaviour
{

    // DatabaseManager.cs�� GetDialogues() �Լ��� ����ϱ� ���� ����
    [SerializeField] DatabaseManager databaseManager;
    private Dialogue[] nowDialogue;
    int count = 0;
    int num = 0;
    [Header("UI")]
    [SerializeField] private GameObject imgBoss; // ���� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] private GameObject imgPlayer; // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] private GameObject imgDialogue; // ��ȭâ �̹���
    [SerializeField] private GameObject uiTextName; // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
    [SerializeField] private GameObject uiTextDialogue; // ĳ���Ͱ� ��� ��� ������Ʈ

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textName; // ��ȭ�ϴ� ĳ������ �̸� text
    [SerializeField] private TextMeshProUGUI textContext; // ĳ������ ��� text

    /*
        /// <summary>
        /// �̺�Ʈ�� �����ϴ� ��ȣ�� ������ ��ȣ�� ���� Text�� �˸��� ��ġ�� ����ϰ�
        /// �̹����� �ٲٴ� �Լ�
        /// </summary>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        public void NowDialogueArr(int startNum, int endNum)
        {
            // nowStart�� �����ͺ��̽����� ����� �迭�� ������ �ҷ���
            Dialogue[] nowStart = databaseManager.GetDialogues(startNum, endNum);

            int i = 0; // �迭�� ��������

            // do - while ������ ������ Ȯ���ϸ鼭 �ݺ�
            do // ������ ù��° �迭�� ���
            {
                Debug.Log(nowStart[i].eventName);
                Debug.Log(nowStart[i].unitId);
                Debug.Log(nowStart[i].name);
                int j = 0; // nowStart[i]�� �迭�� contexts[j]�� Ž���Ͽ� ����ϱ� ���� ������ ����
                Debug.Log(nowStart[i].contexts[j]);
                //ShowText(nowStart, i, j);
                if (++i < nowStart.Length) // �迭�� ���̰� ����� �ʴ� ������
                {
                    while (nowStart[i].name == "") // �̸��� ������ ��쿡�� ���� ĳ���Ͱ� ���� ��縦 �����
                    {
                        j++;
                        Debug.Log(nowStart[i].contexts[j]);
                        ShowText(nowStart, i, j);
                    }
                }
            } while (nowStart[i-1].eventName != "end"); // nowStart�迭�� �̺�Ʈ�̸����� end�� ������ ���� �� ��� �ݺ����� ����
        }

        private void Start()
        {
            NowDialogueArr(0, 4);
        }
    */

    private void Awake()
    {
        imgBoss.SetActive(true); // ���� ĳ������ ��ȭ �� ��� �̹���
        imgPlayer.SetActive(true); // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
        imgDialogue.SetActive(true); // ��ȭâ �̹���
        uiTextName.SetActive(true); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
        uiTextDialogue.SetActive(true); // ĳ���Ͱ� ��� ��� ������Ʈ
    }

    private void Start()
    {
        nowDialogue = databaseManager.dialogues; // DatabaseManager���� Awake()���� ����� Dialogues �迭�� �ҷ��ͼ� ���
        ShowTextName(nowDialogue, count); // �������ڸ��� �̸� ���
        ShowTextContexts(nowDialogue, count, num); // �������ڸ��� ù ��� ���
    }

    private void Update()
    {
        Debug.Log($"������Ʈ���� : {count}");

        // �켱 Ű�Է����� �۵����� �ʾƼ� �켱 ���콺 ��Ŭ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            count++;
            Debug.Log("Ű �Է�");
            StartCoroutine(ShowTextName());
           // StartCoroutine(ShowTextContexts());

        }

        /* �����̽�Ű�� ������ ������� ������� �����Ͽ����� �����̽��ٷ� �۵����� ������ �߰�
         * �� Ű���� �Է���...�ȵ���? ��?
         * ���� �� ���Ƴ�...?
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("�����̽� �Է�");
            StartCoroutine(ShowText());
            count++;
        }
        */
    }

    IEnumerator ShowTextName()
    {
        if (count >= nowDialogue.Length - 1)
        {
            Debug.Log("����");
            yield return null;
        }
        else
        {
            ShowTextName(nowDialogue, count);
            for (num = 0; num < nowDialogue[count].contexts.Length;)
            {
                ShowTextContexts(nowDialogue, count, num);
                num++;
                if (num >= nowDialogue[count].contexts.Length - 1)
                {
                    Debug.Log("�������");
                    yield return null;
                }
            }
        }
        
        yield return null;
    }

    IEnumerator ShowTextContexts()
    {
        for (num = 0; num < nowDialogue[count].contexts.Length; num++)
        {
            ShowTextContexts(nowDialogue, count, num);
            if (num >= nowDialogue[count].contexts.Length - 1)
            {
                yield return null;
            }
        }

    }

    /// <summary>
    /// ���� �������� count�� �˸��� �̸��� ���
    /// ���⼭ count�� Ű�� �Է��Ͽ� ��縦 �ѱ� Ƚ��
    /// </summary>
    /// <param name="nowDialogue"></param>
    /// <param name="count"></param>
    public void ShowTextName(Dialogue[] nowDialogue, int count)
    {
        textName.text = nowDialogue[count].name.ToString();
    }

    public void ShowTextContexts(Dialogue[] nowDialogue, int count, int num)
    {
        Debug.Log(nowDialogue[count].contexts[num]);
        textContext.text = nowDialogue[count].contexts[num].ToString();
    }

    /// <summary>
    /// ���� �������� count�� �˸´� ĳ����Id�� ã�Ƽ� ID�� �´� ĳ���ʹ� �����ϰ�
    /// �ƴ� ĳ���ʹ� ��Ӱ� ǥ���ϱ�
    /// </summary>
    /// <param name="nowDialogue"></param>
    /// <param name="count"></param>
    public void ShowImgName(Dialogue[] nowDialogue, int count)
    {


    }


}

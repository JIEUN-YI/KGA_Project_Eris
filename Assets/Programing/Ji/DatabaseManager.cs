using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField] string csvFileName; // CSV ������ �̸��� ����
    [SerializeField] public Dialogue[] dialogues; // Dialogue�� dialogues�迭�� ���� DialogueParser�� �����͸� �迭�� ����

    // Dictionary�� <string, Dialogue> �� ����
    // dialogueDic�� �����͸� �����Ͽ� ���
    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public static bool isFinish = false; // ������ �Ľ� �� ����� ������ �Ǿ����� ���θ� Ȯ���� �� �ִ� ����

    private void Awake()
    {
        if (instance == null) // instance�� null ���̸�
        {
            instance = this; // ���� �ν��Ͻ��� ����
            // DatabaseManager�� DialogueParser.cs�� ���� ������Ʈ�� �־� �ѹ��� theParser�� ã�� �� �ֵ��� ����
            DialogueParser theParser = GetComponent<DialogueParser>();

            dialogues = theParser.Parser(csvFileName); // DialogueParser�� Parser�Լ��� ����
            // dialogues�� csv������ �����Ͱ� ���� ���� ��
            /*
            // i�� �迭�� �� �� = Dialogue.cs���� ����ϴ� vector2 line�� ������ �� 
            // x��°���� y��°�� ��縦 ����ϵ��� �Ҷ� �����
            for(int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i, dialogues[i]);
            }
            */
            isFinish = true; // �������� ������ �Ϸ��
        }

    }
    /*
    private void Start()
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            if (i == dialogues.Length - 1)
            {
                Debug.Log("����");
                break;
            }
            Debug.Log(dialogues[i].name);

            for (int j = 0; j < dialogues[i].contexts.Length; j++)
            {
                    Debug.Log(dialogues[i].contexts[j]);
            }
        }
    }
    */
    /*
    /// <summary>
    /// �� �̺�Ʈ�� ���Ǵ� ��� Dialogue�� �������� �Լ�
    /// Dialogue[] : �� �̺�Ʈ�� ���Ǵ� ��簡 �������̹Ƿ� �迭�� ����
    /// StartNum : �̺�Ʈ ��簡 �����ϴ� ��°�� �� �� / ���� dialogueDic<int,strint>���� int���� �� ��
    /// EndNum : �̺�Ʈ ��簡 ������ �ϴ� ��°�� �� �� / ���� dialogueDic<int,strint>���� int���� �� ��
    /// 
    /// ��, Start �̺�Ʈ�� �ʿ��� ���� 0��°���� 5��°�̹Ƿ�
    ///     StartNum = 0, EndNum = 5
    ///     phase1 �̺�Ʈ�� �ʿ��� ���� 6��°���� 10��°
    ///     StartNum = 6, EndNum = 10
    ///     MonsterDie �̺�Ʈ�� �ʿ��� ���� 11��°���� 17��°
    ///     StartNum = 11, EndNum = 17
    /// </summary>
    /// <param name="StartNum"></param>
    /// <param name="EndNum"></param>
    /// <returns></returns>
    public Dialogue[] GetDialogues(int StartNum, int EndNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // ����� ���� �̺�Ʈ ���� �ٸ��� ������ ����Ʈ�� ����

        // �ʿ��� ����� �� �� = EndNum - StartNum + 1
        for(int i = 0; i <= EndNum - StartNum; i++)
        {
            // StartNum�� i���� ���Ͽ� ����ؾ� ���ϴ� ����� ��Ȯ�� ���� dialogueDic�� ���� ����
            dialogueList.Add(dialogueDic[StartNum+i]);
            /*Debug.Log(dialogueList[i].eventName);
            Debug.Log(dialogueList[i].name);
            Debug.Log(dialogueList[i].contexts);
        }

        return dialogueList.ToArray(); // ����Ʈ�� �迭�� ��ȯ�Ͽ� ����ϱ�
    }
*/
}

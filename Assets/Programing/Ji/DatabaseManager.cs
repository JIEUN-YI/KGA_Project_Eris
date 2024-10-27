using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField] string csvFileName; // CSV ������ �̸��� ����

    // Dictionary�� <string, Dialogue> �� ����
    // string = EventName ���� �� ��Ȳ���� �ҷ����� Dialogue�� ��ųʸ�
    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public static bool isFinish = false; // ������ �Ľ� �� ����� ������ �Ǿ����� ���θ� Ȯ���� �� �ִ� ����

    private void Awake()
    {
        if(instance == null) // instance�� null ���̸�
        {
            instance = this; // ���� �ν��Ͻ��� ����
            // DatabaseManager�� DialogueParser.cs�� ���� ������Ʈ�� �־� �ѹ��� theParser�� ã�� �� �ֵ��� ����
            DialogueParser theParser = GetComponent<DialogueParser>();

            Dialogue[] dialogues = theParser.Parser(csvFileName); // DialogueParser�� Parser�Լ��� ����
            // dialogues�� csv������ �����Ͱ� ���� ���� ��

            // i�� �迭�� �� �� = Dialogue.cs���� ����ϴ� vector2 line�� ������ �� 
            // x��°���� y��°�� ��縦 ����ϵ��� �Ҷ� �����
            for(int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i, dialogues[i]);
            }
            isFinish = true; // �������� ������ �Ϸ��
        }

    }

    /// <summary>
    /// �� �̺�Ʈ�� ���Ǵ� ��� Dialogue�� �������� �Լ�
    /// Dialogue[] : �� �̺�Ʈ�� ���Ǵ� ��簡 �������̹Ƿ� �迭�� ����
    /// StartNum : �̺�Ʈ ��簡 �����ϴ� ��°�� �� �� / ���� dialogueDic<int,strint>���� int���� �� ��
    /// EndNum : �̺�Ʈ ��簡 ������ �ϴ� ��°�� �� �� / ���� dialogueDic<int,strint>���� int���� �� ��
    /// 
    /// ��, Start �̺�Ʈ�� �ʿ��� ���� 0��°���� 4��°�̹Ƿ�
    ///     StartNum = 0, EndNum = 4
    ///     phase1 �̺�Ʈ�� �ʿ��� ���� 5��°���� 9��°
    ///     StartNum = 5, EndNum = 9
    ///     MonsterDie �̺�Ʈ�� �ʿ��� ���� 10��°���� 15��°
    ///     StartNum = 10, EndNum = 15
    /// </summary>
    /// <param name="StartNum"></param>
    /// <param name="EndNum"></param>
    /// <returns></returns>
    public Dialogue[] GetDialogues(int StartNum, int EndNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // ����� ���� �̺�Ʈ ���� �ٸ��� ������ ����Ʈ�� ����

        // �ʿ��� ����� �� �� = EndNum - StartNum + 1
        for(int i = 0; i <= EndNum - StartNum + 1; i++)
        {
            // StartNum�� i���� ���Ͽ� ����ؾ� ���ϴ� ����� ��Ȯ�� ���� dialogueDic�� ���� ����
            dialogueList.Add(dialogueDic[StartNum+i]);
        }

        return dialogueList.ToArray(); // ����Ʈ�� �迭�� ��ȯ�Ͽ� ����ϱ�
    }

}

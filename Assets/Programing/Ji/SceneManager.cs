using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager; // DialogueSystem.cs�� �Ҵ�
    // DatabaseManager.cs�� GetDialogues() �Լ��� ����ϱ� ���� ����
    [SerializeField] DatabaseManager databaseManager;
    private Dialogue[] nowDialogue;
    int count = 0;
    /*
    [Header("UI")]
    [SerializeField] private GameObject imgBoss; // ���� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] private GameObject imgPlayer; // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
    */
    [SerializeField] private GameObject imgDialogue; // ��ȭâ �̹���
    [SerializeField] private GameObject uiTextName; // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
    [SerializeField] private GameObject uiTextDialogue; // ĳ���Ͱ� ��� ��� ������Ʈ
    /*
     [Header("Audio")]
     [SerializeField] private AudioSource Dialogue;
     [SerializeField] private AudioSource Phase01;
     [SerializeField] private AudioSource Phase02;
     [SerializeField] private AudioSource BossDie;
     [SerializeField] private AudioSource GameOver;
    */

    private void Awake()
    {
        /*  imgBoss.SetActive(true); // ���� ĳ������ ��ȭ �� ��� �̹���
          imgPlayer.SetActive(true); // �÷��̾� ĳ������ ��ȭ �� ��� �̹���*/
        imgDialogue.SetActive(true); // ��ȭâ �̹���
        uiTextName.SetActive(true); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
        uiTextDialogue.SetActive(true); // ĳ���Ͱ� ��� ��� ������Ʈ
    }

    private void Start()
    {
        nowDialogue = databaseManager.dialogues; // DatabaseManager���� Awake()���� ����� Dialogues �迭�� �ҷ��ͼ� ���
        dialogueManager.ShowTextName(nowDialogue, count); // �������ڸ��� �̸� ���
    }
    /*   private void Update()
       {
           switch (nowSceneState)
           {
               case SceneState.Start:
                   imgBoss.SetActive(true); // ���� ĳ������ ��ȭ �� ��� �̹���
                   uiTextName.SetActive(true); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
                   imgPlayer.SetActive(true); // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
                   uiTextName.SetActive(true); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
                   uiTextDialogue.SetActive(true);
                   dialogueManager.NowDialogue(0, 5);
                   imgBoss.SetActive(false); // ���� ĳ������ ��ȭ �� ��� �̹���
                   uiTextName.SetActive(false); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
                   imgPlayer.SetActive(false); // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
                   uiTextName.SetActive(false); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
                   uiTextDialogue.SetActive(false);
                   nowSceneState = SceneState.Phase1Attack;
                   break;
               case SceneState.Phase1Attack:
                   break;
               case SceneState.Phase1Talk:

                   break;
               case SceneState.Phase2:
                   break;
               case SceneState.MonsterDie:

                   break;
           }

       }*/
    private void Update()
    {
        Debug.Log($"������Ʈ���� : {count}");

        // �켱 Ű�Է����� �۵����� �ʾƼ� �켱 ���콺 ��Ŭ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Ű �Է�");
            StartCoroutine(ShowText());
            count++;
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

    IEnumerator ShowText()
    {
        if (count >= nowDialogue.Length - 1)
        {
            Debug.Log("����");
            yield return null;
        }
        else
        {
            dialogueManager.ShowTextName(nowDialogue, count);
        }

        for (int num = 0; num < nowDialogue[count].contexts.Length; num++)
        {
            dialogueManager.ShowTextContexts(nowDialogue, count, num);
        }

        yield return null;
    }



}

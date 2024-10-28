using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Scene�� ���¸� ����
    // ���ӽ��� / 1������ ���� / 1������ ���� / 2������ ���� / ������ ��� / �÷��̾��� ���
    public enum SceneState { Start, Phase1Attack, Phase1Talk, Phase2, MonsterDie, PlayerDie };

    public SceneState nowSceneState; // ���� ���� ���¸� ����

    [SerializeField] DialogueManager dialogueManager; // DialogueSystem.cs�� �Ҵ�
    /*
    [Header("UI")]
    [SerializeField] private GameObject imgBoss; // ���� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] private GameObject imgPlayer; // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
    */
    [SerializeField] private GameObject imgDialogue; // ��ȭâ �̹���
    [SerializeField] private GameObject uiTextName; // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
    [SerializeField] private GameObject uiTextDialogue; // ĳ���Ͱ� ��� ��� ������Ʈ
   
    [Header("Audio")]
    [SerializeField] private AudioSource Dialogue;
    [SerializeField] private AudioSource Phase01;
    [SerializeField] private AudioSource Phase02;
    [SerializeField] private AudioSource BossDie;
    [SerializeField] private AudioSource GameOver;
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
        nowSceneState = SceneState.Start; // ���� �������ڸ��� Start ���·� ����
    }

    private void Update()
    {
       
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


}

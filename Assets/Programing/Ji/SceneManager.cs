using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Scene�� ���¸� ����
    public enum SceneState { Start, Phase1Attack, Phase1Talk, Phase2, MonsterDie };
    /* Start : Scene�� ���� ����, �������ڸ����� ����
     * Phase1 : ��縦 ���� ����� �� Phase1 ����
     *          - Phase1�̸鼭 ������ ���°� Phase2�� ���� �� Phase2�� ����
     *          - Boss�� Phase�� ������ �и�����, �ִϸ��̼ǰ� ���·θ� �и������� ���� �޶�������
     * Phase2 : Phase2�� �����ڸ��� ������ ��� ��ũ��Ʈ�� ���
     *          - Phase2�� ��, ������ ����ϸ� ��� ��ũ��Ʈ�� ����ؾ� ��
     */
    public SceneState nowSceneState; // ���� ���� ���¸� ����

    [SerializeField] DialogueManager dialogueManager; // DialogueSystem.cs�� �Ҵ�

    [Header("UI")]
    [SerializeField] private GameObject imgBoss; // ���� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] private GameObject imgPlayer; // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] private GameObject imgDialogue; // ��ȭâ �̹���
    [SerializeField] private GameObject uiTextName; // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
    [SerializeField] private GameObject uiTextDialogue; // ĳ���Ͱ� ��� ��� ������Ʈ

    private void Awake()
    {
        imgBoss.SetActive(false); // ���� ĳ������ ��ȭ �� ��� �̹���
        imgPlayer.SetActive(false); // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
        imgDialogue.SetActive(true); // ��ȭâ �̹���
        uiTextName.SetActive(false); // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
        uiTextDialogue.SetActive(false); // ĳ���Ͱ� ��� ��� ������Ʈ
        /*
        imgBoss = dialogueManager.imgBoss; // ���� ĳ������ ��ȭ �� ��� �̹���
        imgPlayer = dialogueManager.imgPlayer; // �÷��̾� ĳ������ ��ȭ �� ��� �̹���
        imgDialogue = dialogueManager.imgDialogue; // ��ȭâ �̹���
        uiTextName = dialogueManager.uiTextName; // ��ȭ�ϴ� ĳ������ �̸� ��� ������Ʈ
        uiTextDialogue = dialogueManager.uiTextDialogue; // ĳ���Ͱ� ��� ��� ������Ʈ
        */
    }

    private void Start()
    {
        nowSceneState = SceneState.Start; // ���� �������ڸ��� Start ���·� ����
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
